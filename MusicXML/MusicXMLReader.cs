/* ----------------------------------------------------------------------------
Transonic Score Library
Copyright (C) 1997-2018  George E Greaney

This program is free software; you can redistribute it and/or
modify it under the terms of the GNU General Public License
as published by the Free Software Foundation; either version 2
of the License, or (at your option) any later version.

This program is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU General Public License for more details.

You should have received a copy of the GNU General Public License
along with this program; if not, write to the Free Software
Foundation, Inc., 51 Franklin Street, Fifth Floor, Boston, MA  02110-1301, USA.
----------------------------------------------------------------------------*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.IO.Compression;
using System.Xml;
using System.Xml.Serialization;

using Transonic.Score;
using Transonic.Score.Symbols;

namespace Transonic.Score.MusicXML
{
    public class MusicXMLReader
    {

//- root nodes ----------------------------------------------------------------

        //loads MusicXML file data into a new score doc obj
        public static ScoreDoc loadFromMusicXML(String filename)
        {
            FileStream stream = new FileStream(filename, FileMode.Open);            //get file stream from filename

            XmlReaderSettings settings = new XmlReaderSettings();
            settings.DtdProcessing = DtdProcessing.Parse;
            settings.XmlResolver = null;
            XmlReader reader = XmlReader.Create(stream, settings);                  

            scorepartwise scorePartwise;

            XmlSerializer serializer = new XmlSerializer(typeof(scorepartwise));
            scorePartwise = (scorepartwise)serializer.Deserialize(reader);                  //read xml file into xml tree
            stream.Close();

            ScoreDoc doc = parseScorePartwise(scorePartwise);            //create score doc from xml tree
            doc.filename = filename;
            //doc.dump();

            return doc;
        }

        //a score partwise style file has a score header node followed by a list of part nodes
        public static ScoreDoc parseScorePartwise(scorepartwise scorePartwise)
        {
            ScoreDoc doc = new ScoreDoc();
            parseScoreHeaderXML(scorePartwise, doc);
            foreach (scorepartwisePart scorepart in scorePartwise.part)
            {
                parsePartXML(scorepart, doc);                    
            }
            return doc;
        }

        //not handled yet
        private static ScoreDoc parseScoreTimewise(ScoreSheet sheet, ScoreTimewise scoreXML)
        {
            throw new NotImplementedException();
        }

        //each part node contains a list of measure node
        public static void parsePartXML(scorepartwisePart scorepart, ScoreDoc score)
        {
            Part part = new Part(score, scorepart.id);
            score.parts.Add(part);
            score.curPart = part;

            Measure prevMeasure = null;                                             //for linking measure objs together
            foreach (scorepartwisePartMeasure measure in scorepart.measure)
            {
                    prevMeasure = MusicXMLReader.parseMeasureXML(measure, part, prevMeasure);
            }
            part.layoutStaves();      //which lays out beats, which lays out symbols
            part.setPos();
        }

        //each measure node contains a list of music data nodes (note/direction/attributes/...)
        public static Measure parseMeasureXML(scorepartwisePartMeasure measurexml, Part part, Measure prevMeasure)
        {
            int number = Convert.ToInt32(measurexml.number);
            Measure measure = new Measure(number, prevMeasure);
            Staff staff = part.staves[0];                                 //temporary assign to one and only staff
            measure.staff = staff;
            staff.measures.Add(measure);

            //music-data list
            decimal beatpos = 0;
            Beat curBeat = measure.getBeat(beatpos);
            decimal chordDuration = 0;
            foreach (object item in measurexml.Items)
            {
                if (item is note)
                {
                    Note note = parseNoteXML((note)item);           //get note obj from note node data
                    if (note.chord)                                 //if note is part of chord
                    {
                        beatpos -= chordDuration;                   //backup to pos of previous note
                        Beat beat = measure.getBeat(beatpos);
                        beat.addSymbol(note);                      //add this note to that beat
                        beatpos += chordDuration;                   //use prev note's duration to go to next beat
                    }
                    else
                    {
                        Beat beat = measure.getBeat(beatpos);
                        beat.addSymbol(note);                       //add note obj to cur beat
                        beatpos += note.duration;
                        chordDuration = note.duration;              //if this is the first note of a chord, save its duration
                    }
                }
                else if (item is backup)
                {
                    decimal backup = parseBackupXML((backup)item);
                    beatpos -= backup;                    
                }
                else if (item is forward)
                {
                    decimal forward = parseForwardXML((forward)item);
                    beatpos += forward;                    
                }
                else if (item is direction)
                {
                    Direction direction = parseDirectionXML((direction)item);
                    Beat beat = measure.getBeat(beatpos);
                    beat.addSymbol(direction);
                }
                else if (item is attributes)
                {
                    Attributes attributes = parseAttributesXML((attributes)item);
                    attributes.beatpos = beatpos;
                    measure.setAttributes(attributes);
                }
                //case "harmony":
                //    Harmony harmony = parseHarmonyXML(dataNode);
                //    break;
                //case "figured-bass":
                //    FiguredBass figuredBass = parseFiguredBassXML(dataNode);
                //    break;
                //case "sound":
                //    Sound sound = parseSoundXML(dataNode);
                //    break;
                else if (item is print)
                {
                    Print print = parsePrintXML((print)item);
                    measure.setPrint(print);
                }
                else if (item is barline)
                {
                    Barline barline = parseBarlineXML((barline)item);
                    measure.setBarLine(barline);
                }
                //case "grouping":
                //    Grouping grouping = parseGroupingXML(dataNode);
                //    break;
                //case "link":
                //    Link link = parseLinkXML(dataNode);
                //    break;
                //case "bookmark":
                //    Bookmark bookmark = parseBookmarkXML(dataNode);
                //    break;
            }

            return measure;
        }

//- note nodes ----------------------------------------------------------------

        public static Note parseNoteXML(note notexml)
        {
            //Console.WriteLine("parsing note node");
            Note note = new Note();

            for (int i = 0; i < notexml.ItemsElementName.Length; i++)
            {
                switch (notexml.ItemsElementName[i])
                {
                    case NoteChoiceType.chord:      //empty type
                        note.chord = true;
                        break;

                    case NoteChoiceType.cue:      //empty type
                        note.cue = true;
                        break;

                    case NoteChoiceType.duration:
                        note.duration = (decimal)notexml.Items[i];
                        break;

                    case NoteChoiceType.grace:
                        parseGraceXML((grace)notexml.Items[i], note);
                        break;

                    case NoteChoiceType.pitch:
                        parsePitchXML((pitch)notexml.Items[i], note);
                        break;
                    
                    case NoteChoiceType.rest:
                        parseRestXML((rest)notexml.Items[i], note);
                        break;

                    case NoteChoiceType.tie:
                        parseTieXML((tie)notexml.Items[i], note);
                        break;

                    case NoteChoiceType.unpitched:       
                        parseUnpitchedXML((unpitched)notexml.Items[i], note);
                        break;

                    default:
                        break;
                }
            }

            if (notexml.instrument != null)
            {
                parseInstrumentXML(notexml.instrument, note);
            }
            parseEditorialVoiceNoteXML(notexml, note);
            if (notexml.type != null)
            {
                parseNoteTypeXML(notexml.type, note);
            }
            if (notexml.dot != null)
            {
                parseEmptyPlacementXML(notexml.dot, note);
            }
            if (notexml.accidental != null)
            {
                parseAccidentalXML(notexml.accidental, note);
            }
            if (notexml.timemodification != null)
            {
                parseTimeModificationXML(notexml.timemodification, note);
            }
            if (notexml.stem != null)
            {
                parseStemXML(notexml.stem, note);
            }
            if (notexml.notehead != null)
            {
                parseNoteheadXML(notexml.notehead, note);
            }
            if (notexml.noteheadtext != null)
            {
                parseNoteheadTextXML(notexml.noteheadtext, note);
            }
            if (notexml.staff != null)
            {
                note.staffnum = Convert.ToInt32(notexml.staff);
            }
            if (notexml.beam != null)
            {
                parseBeamXML(notexml.beam, note);
            }
            if (notexml.notations != null)
            {
                parseNotationsXML(notexml.notations, note);
            }
            if (notexml.lyric != null)
            {
                parseLyricXML(notexml.lyric, note);
            }
            if (notexml.play != null)
            {
                parsePlayXML(notexml.play, note);
            }

            return note;
        }

        public static void parseGraceXML(grace gracexml, Note note)
        {            
        }

        static int[] StepToPitch = { 0, 2, 4, 5, 7, 9, 11 };

        public static void parsePitchXML(pitch pitchxml, Note note)
        {
            note.octave = Convert.ToInt32(pitchxml.octave) + 1;
            switch (pitchxml.step)
            {
                case step.C: note.step = 0; break;
                case step.D: note.step = 1; break;
                case step.E: note.step = 2; break;
                case step.F: note.step = 3; break;
                case step.G: note.step = 4; break;
                case step.A: note.step = 5; break;
                case step.B: note.step = 6; break;
                default: break;
            }
            if (pitchxml.alterSpecified)
            {
                note.alter = (int)Math.Truncate(pitchxml.alter);
                note.semitones = note.alter - Math.Truncate(pitchxml.alter);
            }
            note.notenum = (note.octave * 12) + StepToPitch[note.step] + note.alter;            
        }

        public static void parseRestXML(rest restxml, Note note)
        {
            note.rest = true;
        }

        public static void parseTieXML(tie tiexml, Note note)
        {
        }

        public static void parseUnpitchedXML(unpitched unpitchedxml, Note note)
        {

        }

        public static void parseInstrumentXML(instrument instrumentxml, Note note)
        {            
        }

        public static void parseEditorialVoiceNoteXML(note notexml, Note note)
        {
        //    //if (childnodes[num].Name.Equals("footnote"))
        //    //{
        //    //    num = parseFootnoteXML(num, childnodes, note);
        //    //}
        //    //if (childnodes[num].Name.Equals("level"))
        //    //{
        //    //    num = parseLevelXML(num, childnodes, note);
        //    //}
        //    //if (childnodes[num].Name.Equals("voice"))
        //    //{
        //    //    num = parseVoiceXML(num, childnodes, note);
        //    //}
        //    //return num;
        }

        public static void parseNoteTypeXML(notetype notetypexml, Note note)
        {
            if (notetypexml.sizeSpecified)
            {
                note.notesize = (Note.NOTESIZE)notetypexml.size;
            }
            note.notetype = (Note.NOTETYPE)notetypexml.Value;
        }

        public static void parseEmptyPlacementXML(emptyplacement[] emptyplacementxml, Note note)
        {        
        }

        public static void parseAccidentalXML(accidental accidentalxml, Note note)
        {         
        }

        public static void parseTimeModificationXML(timemodification timemodificationxml, Note note)
        {        
        }

        public static void parseStemXML(stem stemxml, Note note)
        {        
        }

        public static void parseNoteheadXML(notehead noteheadxml, Note note)
        {         
        }

        public static void parseNoteheadTextXML(noteheadtext noteheadtextxml, Note note)
        {            
        }

        public static void parseBeamXML(beam[] beamxml, Note note)
        {         
        }

        public static void parseNotationsXML(notations[] notationsxml, Note note)
        {         
        }

        public static void parseLyricXML(lyric[] lyricxml, Note note)
        {         
        }

        public static void parsePlayXML(play playxml, Note note)
        {        
        }

//- complex types -------------------------------------------------------------

        public static AccidentalMark parseAccidentalMarkXML(XmlNode node)
        {
            return null;
        }

        public static AccidentalText parseAccidentalTextXML(XmlNode node)
        {
            return null;
        }

        public static Accord parseAccordXML(XmlNode node)
        {
            return null;
        }

        public static AccordionRegistration parseAccordionRegistrationXML(XmlNode node)
        {
            return null;
        }

        public static Appearance parseAppearanceXML(XmlNode node)
        {
            return null;
        }

        public static Arpeggiate parseArpeggiateXML(XmlNode node)
        {
            return null;
        }

        public static Arrow parseArrowXML(XmlNode node)
        {
            return null;
        }

        public static Articulations parseArticulationsXML(XmlNode node)
        {
            return null;
        }

//- attribute nodes -----------------------------------------------------------

        public static Attributes parseAttributesXML(attributes attrsxml)
        {
            //Console.WriteLine("parsing attributes node");
            Attributes attrs = new Attributes();

            if (attrsxml.divisionsSpecified)
            {
                attrs.divisions = attrsxml.divisions;
                attrs.haveDivisions = true;
            }
            if (attrsxml.key != null)
            {
                parseKeyXML(attrsxml.key[0], attrs);
            }
            if (attrsxml.time != null)
            {
                parseTimeXML(attrsxml.time[0], attrs);
            }
            
            //attrs.editorial = parseEditorialXML(ref num, childnodes);    
            //if ((num < count) && childnodes[num].Name.Equals("staves"))
            //{
            //    attrs.staves = Convert.ToInt32(childnodes[num].Value);
            //};
            //if ((num < count) && childnodes[num].Name.Equals("part-symbol"))
            //{
            //    attrs.partSymbol = parsePartSymbolXML(childnodes[num++]);
            //};
            //if ((num < count) && childnodes[num].Name.Equals("instrument"))
            //{
            //    attrs.instrument = Convert.ToInt32(childnodes[num].Value);
            //};
            //while ((num < count) && childnodes[num].Name.Equals("clef"))
            //{
            //    attrs.clefs.Add(parseClefXML(childnodes[num++]));
            //};
            //while ((num < count) && childnodes[num].Name.Equals("staff-details"))
            //{
            //    attrs.staffdetails.Add(parseStaffDetailsXML(childnodes[num++]));
            //};
            //while ((num < count) && childnodes[num].Name.Equals("transpose"))
            //{
            //    attrs.transposes.Add(parseTransposeXML(childnodes[num++]));
            //};
            //while ((num < count) && childnodes[num].Name.Equals("measure-style"))
            //{
            //    attrs.measurestyles.Add(parseMeasureStyleXML(childnodes[num++]));
            //};
            return attrs;
        }

        public static void parseKeyXML(key keyxml, Attributes attrs)
        {
            for (int i = 0; i < keyxml.ItemsElementName.Length; i++)
            {
                switch (keyxml.ItemsElementName[i])
                {
                    case KeyChoiceTypes.fifths:
                        attrs.key = Convert.ToInt32((string)keyxml.Items[i]);
                        break;
                    case KeyChoiceTypes.mode:
                        switch ((string)keyxml.Items[i])
                        {
                            case "major" : attrs.mode = KEYMODE.Major; break;
                            case "minor": attrs.mode = KEYMODE.Minor; break;
                            default: break;
                        }
                        break;
                    default:
                        break;
                }
            }
        }

        //public static KeyAccidental parseKeyAccidentalXML(XmlNode node)
        //{
        //    return null;
        //}

        //public static KeyOctave parseKeyOctaveXML(XmlNode node)
        //{
        //    return null;
        //}

        //public static Cancel parseCancelXML(XmlNode node)
        //{
        //    return null;
        //}

        public static void parseTimeXML(time timexml, Attributes attrs)
        {
            for (int i = 0; i < timexml.ItemsElementName.Length; i++)
            {
                switch (timexml.ItemsElementName[i])
                {
                    case TimeChoiceTypes.beats:
                        attrs.timeNumer = Convert.ToInt32((string)timexml.Items[i]);
                        break;
                    case TimeChoiceTypes.beattype:
                        attrs.timeDenom = Convert.ToInt32((string)timexml.Items[i]);
                        break;
                    default:
                        break;
                }
            }
        }

        //public static Interchangeable parseInterchangeableXML(XmlNode node)
        //{
        //    return null;
        //}

        //public static PartSymbol parsePartSymbolXML(XmlNode node)
        //{
        //    return null;
        //}

        //public static Clef parseClefXML(XmlNode node)
        //{
        //    return null;
        //}

        //public static StaffDetails parseStaffDetailsXML(XmlNode node)
        //{
        //    return null;
        //}

        //public static Staff parseStafftuningXML(XmlNode node)
        //{
        //    return null;
        //}

        //public static Transpose parseTransposeXML(XmlNode node)
        //{
        //    return null;
        //}

        //public static Slash parseSlashXML(XmlNode node)
        //{
        //    return null;
        //}

        //public static MeasureStyle parseMeasureStyleXML(XmlNode node)
        //{
        //    return null;
        //}

        //public static BeatRepeat parseBeatRepeatXML(XmlNode node)
        //{
        //    return null;
        //}

        //public static MeasureRepeat parseMeasureRepeatXML(XmlNode node)
        //{
        //    return null;
        //}

        //public static MultipleRest parseMultipleRestXML(XmlNode node)
        //{
        //    return null;
        //}

//-----------------------------------------------------------------------------

        public static decimal parseBackupXML(backup backupxml)
        {
            //Console.WriteLine("parsing backup node");
            return backupxml.duration;
        }

        public static decimal parseForwardXML(forward forwardxml)
        {
            //Console.WriteLine("parsing forward node");
            return forwardxml.duration;
        }

        public static BarStyleColor parseBarStyleColorXML(XmlNode node)
        {
            return null;
        }

        public static Barline parseBarlineXML(barline item)
        {
            //Console.WriteLine("parsing barline node");
            return null;
        }

        public static Barre parseBarreXML(XmlNode node)
        {
            return null;
        }

        public static Bass parseBassXML(XmlNode node)
        {
            return null;
        }

        public static BassAlter parseBassAlterXML(XmlNode node)
        {
            return null;
        }

        public static BassStep parseBassStepXML(XmlNode node)
        {
            return null;
        }

        public static BeatUnitTied parseBeatUnitTiedXML(XmlNode node)
        {
            return null;
        }

        public static Beater parseBeaterXML(XmlNode node)
        {
            return null;
        }

        public static Bend parseBendXML(XmlNode node)
        {
            return null;
        }

        public static Bookmark parseBookmarkXML(XmlNode node)
        {
            //Console.WriteLine("parsing bookmark node");
            return null;
        }

        public static Bracket parseBracketXML(XmlNode node)
        {
            return null;
        }

        public static BreathMark parseBreathMarkXML(XmlNode node)
        {
            return null;
        }

        public static Caesura parseCaesuraXML(XmlNode node)
        {
            return null;
        }

        public static Coda parseCodaXML(XmlNode node)
        {
            return null;
        }

        public static Credit parseCreditXML(XmlNode node)
        {
            return null;
        }

        public static Dashes parseDashesXML(XmlNode node)
        {
            return null;
        }

        public static Defaults parseDefaultsXML(XmlNode node)
        {
            return null;
        }

        public static Degree parseDegreeXML(XmlNode node)
        {
            return null;
        }

        public static DegreeAlter parseDegreeAlterXML(XmlNode node)
        {
            return null;
        }

        public static DegreeType parseDegreeTypeXML(XmlNode node)
        {
            return null;
        }

        public static DegreeValue parseDegreeValueXML(XmlNode node)
        {
            return null;
        }

        public static Direction parseDirectionXML(direction item)
        {
            //Console.WriteLine("parsing direction node");            
            return null;
        }

        public static Directiontype parseDirectionTypeXML(XmlNode node)
        {
            return null;
        }

        public static Distance parseDistanceXML(XmlNode node)
        {
            return null;
        }

        public static Dynamics parseDynamicsXML(XmlNode node)
        {
            return null;
        }

        public static Elision parseElisionXML(XmlNode node)
        {
            return null;
        }

        //public static Empty parseEmptyXML(XmlNode node)
        //{
        //    return null;
        //}

        public static EmptyFont parseEmptyFontXML(XmlNode node)
        {
            return null;
        }

        public static EmptyLine parseEmptyLineXML(XmlNode node)
        {
            return null;
        }

        public static EmptyPlacementSmufl parseEmptyPlacementSmuflXML(XmlNode node)
        {
            return null;
        }

        public static EmptyPrintObjectStyleAlign parseEmptyPrintObjectStyleAlignXML(XmlNode node)
        {
            return null;
        }

        public static EmptyPrintStyleAlignId parseEmptyPrintStyleAlignIdXML(XmlNode node)
        {
            return null;
        }

        public static EmptyTrillSound parseEmptyTrillSoundXML(XmlNode node)
        {
            return null;
        }

        public static Transonic.Score.Symbols.Encoding parseEncodingXML(XmlNode node)
        {
            return null;
        }

        public static Ending parseEndingXML(XmlNode node)
        {
            return null;
        }

        public static Extend parseExtendXML(XmlNode node)
        {
            return null;
        }

        public static Feature parseFeatureXML(XmlNode node)
        {
            return null;
        }

        public static Fermata parseFermataXML(XmlNode node)
        {
            return null;
        }

        public static Figure parseFigureXML(XmlNode node)
        {
            return null;
        }

        public static FiguredBass parseFiguredBassXML(XmlNode node)
        {
            //Console.WriteLine("parsing figured bass node");
            return null;
        }

        public static Fingering parseFingeringXML(XmlNode node)
        {
            return null;
        }

        public static FirstFret parseFirstFretXML(XmlNode node)
        {
            return null;
        }

        public static FormattedSymbolId parseFormattedSymbolIdXML(XmlNode node)
        {
            return null;
        }

        public static FormattedTextId parseFormattedTextIdXML(XmlNode node)
        {
            return null;
        }

        public static FormattedText parseFormattedTextXML(XmlNode node)
        {
            return null;
        }

        public static FrameNote parseFrameNoteXML(XmlNode node)
        {
            return null;
        }

        public static Frame parseFrameXML(XmlNode node)
        {
            return null;
        }

        public static Fret parseFretXML(XmlNode node)
        {
            return null;
        }

        public static Glass parseGlassXML(XmlNode node)
        {
            return null;
        }

        public static Glissando parseGlissandoXML(XmlNode node)
        {
            return null;
        }

        public static Glyph parseGlyphXML(XmlNode node)
        {
            return null;
        }

        public static GroupBarline parseGroupBarlineXML(XmlNode node)
        {
            return null;
        }

        public static GroupName parseGroupNameXML(XmlNode node)
        {
            return null;
        }

        public static GroupSymbol parseGroupSymbolXML(XmlNode node)
        {
            return null;
        }

        public static Grouping parseGroupingXML(XmlNode node)
        {
            //Console.WriteLine("parsing grouping node");
            return null;
        }

        public static HammerOnPullOff parseHammerOnPullOffXML(XmlNode node)
        {
            return null;
        }

        public static Handbell parseHandbellXML(XmlNode node)
        {
            return null;
        }

        public static HarmonClosed parseHarmonClosedXML(XmlNode node)
        {
            return null;
        }

        public static HarmonMute parseHarmonMuteXML(XmlNode node)
        {
            return null;
        }

        public static Harmonic parseHarmonicXML(XmlNode node)
        {
            return null;
        }

        public static Harmony parseHarmonyXML(XmlNode node)
        {
            //Console.WriteLine("parsing harmony node");
            return null;
        }

        public static HarpPedals parseHarpPedalsXML(XmlNode node)
        {
            return null;
        }

        public static HeelToe parseHeelToeXML(XmlNode node)
        {
            return null;
        }

        public static Hole parseHoleXML(XmlNode node)
        {
            return null;
        }

        public static HoleClosed parseHoleClosedXML(XmlNode node)
        {
            return null;
        }

        public static HorizontalTurn parseHorizontalTurnXML(XmlNode node)
        {
            return null;
        }

        public static Identification parseIdentificationXML(XmlNode node)
        {
            return null;
        }

        public static Image parseImageXML(XmlNode node)
        {
            return null;
        }

        public static Inversion parseInversionXML(XmlNode node)
        {
            return null;
        }

        public static Kind parseKindXML(XmlNode node)
        {
            return null;
        }

        public static Level parseLevelXML(XmlNode node)
        {
            return null;
        }

        public static LineWidth parseLineWidthXML(XmlNode node)
        {
            return null;
        }

        public static Link parseLinkXML(XmlNode node)
        {
            //Console.WriteLine("parsing link node");
            return null;
        }

        public static LyricFont parseLyricFontXML(XmlNode node)
        {
            return null;
        }

        public static LyricLanguage parseLyricLanguageXML(XmlNode node)
        {
            return null;
        }

        public static MeasureLayout parseMeasureLayoutXML(XmlNode node)
        {
            return null;
        }

        public static MeasureNumbering parseMeasureNumberingXML(XmlNode node)
        {
            return null;
        }

        public static Metronome parseMetronomeXML(XmlNode node)
        {
            return null;
        }

        public static MetronomeBeam parseMetronomeBeamXML(XmlNode node)
        {
            return null;
        }

        public static MetronomeNote parseMetronomeNoteXML(XmlNode node)
        {
            return null;
        }

        public static MetronomeTied parseMetronomeTiedXML(XmlNode node)
        {
            return null;
        }

        public static MetronomeTuplet parseMetronomeTupletXML(XmlNode node)
        {
            return null;
        }

        public static MidiDevice parseMidiDeviceXML(XmlNode node)
        {
            return null;
        }

        public static MidiInstrument parseMidiInstrumentXML(XmlNode node)
        {
            return null;
        }

        public static Miscellaneous parseMiscellaneousXML(XmlNode node)
        {
            return null;
        }

        public static MiscellaneousField parseMiscellaneousFieldXML(XmlNode node)
        {
            return null;
        }

        public static Mordent parseMordentXML(XmlNode node)
        {
            return null;
        }

        public static NameDisplay parseNameDisplayXML(XmlNode node)
        {
            return null;
        }

        public static NonArpeggiate parseNonArpeggiateXML(XmlNode node)
        {
            return null;
        }

        public static NoteSize parseNoteSizeXML(XmlNode node)
        {
            return null;
        }

        public static OctaveShift parseOctaveShiftXML(XmlNode node)
        {
            return null;
        }

        public static Offset parseOffsetXML(XmlNode node)
        {
            return null;
        }

        public static Opus parseOpusXML(XmlNode node)
        {
            return null;
        }

        public static Ornaments parseOrnamentsXML(XmlNode node)
        {
            return null;
        }

        public static OtherAppearance parseOtherAppearanceXML(XmlNode node)
        {
            return null;
        }

        public static OtherDirection parseOtherDirectionXML(XmlNode node)
        {
            return null;
        }

        public static OtherNotation parseOtherNotationXML(XmlNode node)
        {
            return null;
        }

        public static OtherPlacementText parseOtherPlacementTextXML(XmlNode node)
        {
            return null;
        }

        public static OtherPlay parseOtherPlayXML(XmlNode node)
        {
            return null;
        }

        public static OtherText parseOtherTextXML(XmlNode node)
        {
            return null;
        }

        public static PageLayout parsePageLayoutXML(XmlNode node)
        {
            return null;
        }

        public static PageMargins parsePageMarginsXML(XmlNode node)
        {
            return null;
        }

        public static PartGroup parsePartGroupXML(XmlNode node)
        {
            return null;
        }

        public static PartList parsePartListXML(XmlNode node)
        {
            return null;
        }

        public static PartName parsePartNameXML(XmlNode node)
        {
            return null;
        }

        public static Pedal parsePedalXML(XmlNode node)
        {
            return null;
        }

        public static PedalTuning parsePedalTuningXML(XmlNode node)
        {
            return null;
        }

        public static PerMinute parsePerMinuteXML(XmlNode node)
        {
            return null;
        }

        public static Percussion parsePercussionXML(XmlNode node)
        {
            return null;
        }

        //private static void parsePitchXML(XmlNode pitchNode, Note note)
        //{
        //    int num = 0;
        //    XmlNodeList childnodes = pitchNode.ChildNodes;
        //    string stepstr = childnodes[num++].InnerText.ToUpper();
        //    note.step = stepstr[0] - 'A';
        //    if (childnodes[num].Name.Equals("alter"))
        //    {
        //        note.alter = Convert.ToDouble(childnodes[num++].InnerText);
        //    }
        //    note.octave = Convert.ToInt32(childnodes[num].InnerText);
        //}
        
        public static Pitched parsePitchedXML(XmlNode node)
        {
            return null;
        }

        public static PlacementText parsePlacementTextXML(XmlNode node)
        {
            return null;
        }

        public static PrincipalVoice parsePrincipalVoiceXML(XmlNode node)
        {
            return null;
        }

        public static Print parsePrintXML(print item)
        {
            //Console.WriteLine("parsing print node");
            return null;
        }

        public static Repeat parseRepeatXML(XmlNode node)
        {
            return null;
        }

        //private static void parseRestXML(XmlNode restNode, Note note)
        //{
        //    throw new NotImplementedException();
        //}

        public static Root parseRootXML(XmlNode node)
        {
            return null;
        }

        public static RootAlter parseRootAlterXML(XmlNode node)
        {
            return null;
        }

        public static RootStep parseRootStepXML(XmlNode node)
        {
            return null;
        }

        public static Scaling parseScalingXML(XmlNode node)
        {
            return null;
        }

        public static Scordatura parseScordaturaXML(XmlNode node)
        {
            return null;
        }

        public static ScoreInstrument parseScoreInstrumentXML(XmlNode node)
        {
            return null;
        }

        public static ScorePart parseScorePartXML(XmlNode node)
        {
            return null;
        }

        public static Segno parseSegnoXML(XmlNode node)
        {
            return null;
        }

        public static Slide parseSlideXML(XmlNode node)
        {
            return null;
        }

        public static Slur parseSlurXML(XmlNode node)
        {
            return null;
        }

        public static Sound parseSoundXML(XmlNode node)
        {
            //Console.WriteLine("parsing sound node");
            return null;
        }

        public static StaffDivide parseStaffDivideXML(XmlNode node)
        {
            return null;
        }

        public static StaffLayout parseStaffLayoutXML(XmlNode node)
        {
            return null;
        }

        public static Stick parseStickXML(XmlNode node)
        {
            return null;
        }

        public static StringSym parseStringXML(XmlNode node)
        {
            return null;
        }

        public static StringMute parseStringMuteXML(XmlNode node)
        {
            return null;
        }

        public static StrongAccent parseStrongAccentXML(XmlNode node)
        {
            return null;
        }

        public static StyleText parseStyleTextXML(XmlNode node)
        {
            return null;
        }

        public static Supports parseSupportsXML(XmlNode node)
        {
            return null;
        }

        public static SystemDividers parseSystemDividersXML(XmlNode node)
        {
            return null;
        }

        public static SystemLayout parseSystemLayoutXML(XmlNode node)
        {
            return null;
        }

        public static SystemMargins parseSystemMarginsXML(XmlNode node)
        {
            return null;
        }

        public static Tap parseTapXML(XmlNode node)
        {
            return null;
        }

        public static Technical parseTechnicalXML(XmlNode node)
        {
            return null;
        }

        public static TextElementData parseTextElementDataXML(XmlNode node)
        {
            return null;
        }

        public static Tie parseTieXML(XmlNode node)
        {
            return null;
        }

        public static Tied parseTiedXML(XmlNode node)
        {
            return null;
        }

        public static Tremolo parseTremoloXML(XmlNode node)
        {
            return null;
        }

        public static Tuplet parseTupletXML(XmlNode node)
        {
            return null;
        }

        public static TupletDot parseTupletDotXML(XmlNode node)
        {
            return null;
        }

        public static TupletNumber parseTupletNumberXML(XmlNode node)
        {
            return null;
        }

        public static TupletPortion parseTupletPortionXML(XmlNode node)
        {
            return null;
        }

        public static TupletType parseTupletTypeXML(XmlNode node)
        {
            return null;
        }

        public static TypedText parseTypedTextXML(XmlNode node)
        {
            return null;
        }

        //private static void parseUnpitchedXML(XmlNode unpitchedNode, Note note)
        //{
        //    throw new NotImplementedException();
        //}

        public static VirtualInstrument parseVirtualInstrumentXML(XmlNode node)
        {
            return null;
        }

        public static WavyLine parseWavyLineXML(XmlNode node)
        {
            return null;
        }

        public static Wedge parseWedgeXML(XmlNode node)
        {
            return null;
        }

        public static Work parseWorkXML(XmlNode node)
        {
            return null;
        }

//- element groups -------------------------------------------------------------

        public static AllMargins parseAllMarginsXML(ref int num, XmlNodeList childnodes)
        {
            return null;
        }
        
        public static BeatUnit parseBeatUnitXML(ref int num, XmlNodeList childnodes)
        {
            return null;
        }


        public static DisplayStepOctave parseDisplayStepOctaveXML(ref int num, XmlNodeList childnodes)
        {
            return null;
        }

        public static Duration parseDurationXML(ref int num, XmlNodeList childnodes)
        {
            return null;
        }

        public static Editorial parseEditorialXML(ref int num, XmlNodeList childnodes)
        {
            return null;
        }

        public static EditorialVoice parseEditorialVoiceXML(ref int num, XmlNodeList childnodes)
        {
            return null;
        }

        public static EditorialVoiceDirection parseEditorialVoiceDirectionXML(ref int num, XmlNodeList childnodes)
        {
            return null;
        }

        public static Footnote parseFootnoteXML(ref int num, XmlNodeList childnodes)
        {
            return null;
        }

        public static FullNote parseFullNoteXML(ref int num, XmlNodeList childnodes)
        {
            //int count = childnodes.Count;
            //if ((num < count) && childnodes[num].Name.Equals("chord"))
            //{
            //    note.chord = true;
            //    num++;
            //};
            //if (childnodes[num].Name.Equals("pitch"))
            //{
            //    parsePitchXML(childnodes[num++], note);
            //}
            //else if (childnodes[num].Name.Equals("unpitched"))
            //{
            //    parseUnpitchedXML(childnodes[num++], note);
            //}
            //else
            //{
            //    parseRestXML(childnodes[num++], note);
            //}
            //return num;
            return null;
        }

        public static HarmonyChord parseHarmonyChordXML(ref int num, XmlNodeList childnodes)
        {
            return null;
        }

        public static Layout parseLayoutXML(ref int num, XmlNodeList childnodes)
        {
            return null;
        }

        public static LeftRightMargins parseLeftRightMarginsXML(ref int num, XmlNodeList childnodes)
        {
            return null;
        }

        public static LevelGroup parseLevelXML(ref int num, XmlNodeList childnodes)
        {
            return null;
        }

        public static MusicData parseMusicDataXML(ref int num, XmlNodeList childnodes)
        {
            return null;
        }

        public static NonTraditionalKey parseNonTraditionalKeyXML(ref int num, XmlNodeList childnodes)
        {
            return null;
        }

        public static PartGroup parsePartGroupXML(ref int num, XmlNodeList childnodes)
        {
            return null;
        }

        public static void parseScoreHeaderXML(scorepartwise scorePartwise, ScoreDoc doc)
        {
            //int count = (childnodes != null) ? childnodes.Count : 0;

            //if ((num < count) && (childnodes[num].Name.Equals("work")))
            //{
            //    doc.work = parseWorkXML(childnodes[num++]);
            //};
            //if ((num < count) && (childnodes[num].Name.Equals("identification")))
            //{
            //    doc.identification = parseIdentificationXML(childnodes[num++]);
            //};
            //if ((num < count) && (childnodes[num].Name.Equals("defaults")))
            //{
            //    doc.defaults = parseDefaultsXML(childnodes[num++]);
            //};
            //while ((num < count) && (childnodes[num].Name.Equals("credit")))
            //{
            //    doc.credits.Add(parseCreditXML(childnodes[num++]));
            //};
            //if ((num < count) && (childnodes[num].Name.Equals("part-list")))
            //{
            //    doc.partList = parsePartListXML(childnodes[num++]);
            //}
            //else
            //{
            //    throw new MusicXMLReadException("MusicMXL error - missing part-list from score-header group");
            //}
        }

        public static ScorePart parseScorePartXML(ref int num, XmlNodeList childnodes)
        {
            return null;
        }

        //public static Slash parseSlashXML(ref int num, XmlNodeList childnodes)
        //{
        //    return null;
        //}

        public static TimeSignature parseTimeSignatureXML(ref int num, XmlNodeList childnodes)
        {
            return null;
        }

        public static TraditionalKey parseTraditionalKeyXML(ref int num, XmlNodeList childnodes)
        {
            return null;
        }

        public static Tuning parseTuningXML(ref int num, XmlNodeList childnodes)
        {
            return null;
        }

        public static Voice parseVoiceXML(ref int num, XmlNodeList childnodes)
        {
            return null;
        }

        //private static int parseVoiceXML(int num, XmlNodeList childnodes, Note note)
        //{
        //    note.voice = childnodes[0].Value;
        //    return 1;
        //}
    }

//-----------------------------------------------------------------------------

    public class MusicXMLReadException : Exception
    {
        public MusicXMLReadException(String msg) : base(msg) { }
    }
}
