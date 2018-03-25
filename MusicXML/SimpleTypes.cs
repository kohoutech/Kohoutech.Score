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

namespace Transonic.Score.MusicXML
{
    public class SimpleTypes
    {
        //reading
        //must cast the return int val to matching enum on the calling side
        static public int readSimpleType(SimpleTypeName name, String value)
        {
            String[] vals = SimpleTypeTbl[name];
            int i = 0;
            for (; i < vals.Length; i++)
            {
                if (value.Equals(vals[i],StringComparison.OrdinalIgnoreCase)) {
                    break;
                }
            }
            return i;
        }

        //writing
        static public void writeSimpleType(SimpleTypeName name, int value)
        {
            String strval = SimpleTypeTbl[name][value];
            
            //write out in some manner
        }

        public enum SimpleTypeName
        {
            ABOVEBELOW, ACCIDENTALVALUE, ARROWDIRECTION, ARROWSTYLE, BACKWARDFORWARD, BARSTYLE, BEAMVALUE, BEATERVALUE,
            BREATHMARKVALUE, CAESURAVALUE, CANCELLOCATION, CIRCULARARROW, CLEFSIGN, CSSFONTSIZE, DEGREESYMBOLVALUE, DEGREETYPEVALUE,
            EFFECT, ENCLOSURESHAPE, FAN, FERMATASHAPE, FONTSTYLE, FONTWEIGHT, GLASSVALUE, GROUPBARLINEVALUE, GROUPSYMBOLVALUE,
            HANDBELLVALUE, HARMONCLOSEDLOCATION, HARMONCLOSEDVALUE, HARMONYTYPE, HOLECLOSEDLOCATION, HOLECLOSEDVALUE, KINDVALUE,
            LEFTCENTERRIGHT, LEFTRIGHT, LINEEND, LINELENGTH, LINESHAPE, LINETYPE, MARGINTYPE, MEASURENUMBERINGVALUE, MEMBRANE,
            METAL, MUTE, NOTESIZETYPE, NOTETYPEVALUE, NOTEHEADVALUE, OVERUNDER, PEDALTYPE, PITCHEDVALUE, PRINCIPALVOICESYMBOL,
            RIGHTLEFTMIDDLE, SEMIPITCHED, SHOWFRETS, SHOWTUPLET, STAFFDIVIDESYMBOL, STAFFTYPE, STARTNOTE, STARTSTOP,
            STARTSTOPCHANGECONTINUE, STARTSTOPCONTINUE, STARTSTOPDISCONTINUE, STARTSTOPSINGLE, STEMVALUE, STEP, STICKLOCATION,
            STICKMATERIAL, STICKTYPE, SYLLABIC, SYMBOLSIZE, TAPHAND, TEXTDIRECTION, TIEDTYPE, TIMERELATION, TIMESEPARATOR,
            TIMESYMBOL, TIPDIRECTION, TOPBOTTOM, TREMOLOTYPE, TRILLSTEP, TWONOTETURN, UPDOWNSTOPCONTINUE, UPDOWN,
            UPRIGHTINVERTED, VALIGN, VALIGNIMAGE, WEDGETYPE, WINGED, WOOD
        };

        static public Dictionary<SimpleTypeName, String[]> SimpleTypeTbl = new Dictionary<SimpleTypeName, String[]>()
        {
            {SimpleTypeName.ABOVEBELOW, sAboveBelow},
            {SimpleTypeName.ACCIDENTALVALUE, sAccidentalValue},
            {SimpleTypeName.ARROWDIRECTION, sArrowDirection},
            {SimpleTypeName.ARROWSTYLE, sArrowStyle},
            {SimpleTypeName.BACKWARDFORWARD, sBackwardForward},
            {SimpleTypeName.BARSTYLE, sBarStyle},
            {SimpleTypeName.BEAMVALUE, sBeamValue},
            {SimpleTypeName.BEATERVALUE,sBeaterValue},
            {SimpleTypeName.BREATHMARKVALUE, sBreathMarkValue},
            {SimpleTypeName.CAESURAVALUE, sCaesuraValue},
            {SimpleTypeName.CANCELLOCATION, sCancelLocation},
            {SimpleTypeName.CIRCULARARROW, sCircularArrow},
            {SimpleTypeName.CLEFSIGN, sClefSign},
            {SimpleTypeName.CSSFONTSIZE, sCssFontSize},
            {SimpleTypeName.DEGREESYMBOLVALUE, sDegreeSymbolValue},
            {SimpleTypeName.DEGREETYPEVALUE,sDegreeTypeValue},
            {SimpleTypeName.EFFECT, sEffect},
            {SimpleTypeName.ENCLOSURESHAPE,sEnclosureShape },
            {SimpleTypeName.FAN, sFan},
            {SimpleTypeName.FERMATASHAPE, sFermataShape},
            {SimpleTypeName.FONTSTYLE, sFontStyle},
            {SimpleTypeName.FONTWEIGHT, sFontWeight},
            {SimpleTypeName.GLASSVALUE, sGlassValue},
            {SimpleTypeName.GROUPBARLINEVALUE, sGroupBarlineValue},
            {SimpleTypeName.GROUPSYMBOLVALUE,sGroupSymbolValue},
            {SimpleTypeName.HANDBELLVALUE, sHandbellValue},
            {SimpleTypeName.HARMONCLOSEDLOCATION, sHarmonClosedLocation},
            {SimpleTypeName.HARMONCLOSEDVALUE, sHoleClosedValue},
            {SimpleTypeName.HARMONYTYPE, sHarmonyType},
            {SimpleTypeName.HOLECLOSEDLOCATION, sHoleClosedLocation},
            {SimpleTypeName.HOLECLOSEDVALUE, sHoleClosedValue},
            {SimpleTypeName.KINDVALUE,sKindValue},
            {SimpleTypeName.LEFTCENTERRIGHT, sLeftCenterRight},
            {SimpleTypeName.LEFTRIGHT, sLeftRight},
            {SimpleTypeName.LINEEND, sLineEnd},
            {SimpleTypeName.LINELENGTH, sLineLength},
            {SimpleTypeName.LINESHAPE, sLineShape},
            {SimpleTypeName.LINETYPE, sLineType},
            {SimpleTypeName.MARGINTYPE, sMarginType},
            {SimpleTypeName.MEASURENUMBERINGVALUE, sMeasureNumberingValue},
            {SimpleTypeName.MEMBRANE,sMembrane},
            {SimpleTypeName.METAL, sMetal},
            {SimpleTypeName.MUTE, sMute},
            {SimpleTypeName.NOTESIZETYPE, sNoteSizeType},
            {SimpleTypeName.NOTETYPEVALUE, sNoteTypeValue},
            {SimpleTypeName.NOTEHEADVALUE, sNoteheadValue},
            {SimpleTypeName.OVERUNDER, sOverUnder},
            {SimpleTypeName.PEDALTYPE, sPedalType},
            {SimpleTypeName.PITCHEDVALUE, sPitchedValue},
            {SimpleTypeName.PRINCIPALVOICESYMBOL,sPrincipalVoiceSymbol},
            {SimpleTypeName.RIGHTLEFTMIDDLE, sRightLeftMiddle},
            {SimpleTypeName.SEMIPITCHED, sSemiPitched},
            {SimpleTypeName.SHOWFRETS, sShowFrets},
            {SimpleTypeName.SHOWTUPLET, sShowTuplet},
            {SimpleTypeName.STAFFDIVIDESYMBOL, sStaffDivideSymbol},
            {SimpleTypeName.STAFFTYPE, sStaffType},
            {SimpleTypeName.STARTNOTE, sStartNote},
            {SimpleTypeName.STARTSTOP,sStartStop},
            {SimpleTypeName.STARTSTOPCHANGECONTINUE, sStartStopChangeContinue},
            {SimpleTypeName.STARTSTOPCONTINUE, sStartStopContinue},
            {SimpleTypeName.STARTSTOPDISCONTINUE, sStartStopDiscontinue},
            {SimpleTypeName.STARTSTOPSINGLE, sStartStopSingle},
            {SimpleTypeName.STEMVALUE, sStemValue},
            {SimpleTypeName.STEP, sStep},
            {SimpleTypeName.STICKLOCATION,sStickLocation},
            {SimpleTypeName.STICKMATERIAL, sStickMaterial},
            {SimpleTypeName.STICKTYPE, sStickType},
            {SimpleTypeName.SYLLABIC, sSyllabic},
            {SimpleTypeName.SYMBOLSIZE, sSymbolSize},
            {SimpleTypeName.TAPHAND, sTapHand},
            {SimpleTypeName.TEXTDIRECTION, sTextDirection},
            {SimpleTypeName.TIEDTYPE, sTiedType},
            {SimpleTypeName.TIMERELATION, sTimeRelation},
            {SimpleTypeName.TIMESEPARATOR,sTimeSeparator},
            {SimpleTypeName.TIMESYMBOL, sTimeSymbol},
            {SimpleTypeName.TIPDIRECTION, sTipDirection},
            {SimpleTypeName.TOPBOTTOM, sTopBottom},
            {SimpleTypeName.TREMOLOTYPE, sTremoloType},
            {SimpleTypeName.TRILLSTEP, sTrillStep},
            {SimpleTypeName.TWONOTETURN, sTwoNoteTurn},
            {SimpleTypeName.UPDOWNSTOPCONTINUE, sUpDownStopContinue},
            {SimpleTypeName.UPDOWN,sUpDown},
            {SimpleTypeName.UPRIGHTINVERTED, sUprightInverted},
            {SimpleTypeName.VALIGN, sVAlign},
            {SimpleTypeName.VALIGNIMAGE, sVAlignImage},
            {SimpleTypeName.WEDGETYPE, sWedgeType},
            {SimpleTypeName.WINGED, sWinged},
            {SimpleTypeName.WOOD,sWood}
        };

//- data ----------------------------------------------------------------------

        enum AboveBelow { ABOVE, BELOW };
        static String[] sAboveBelow = { "above", "below" };

        enum AccidentalValue
        {
            SHARP, NATURAL, FLAT, DOUBLESHARP, SHARPSHARP, FLATFLAT, NATURALSHARP, NATURALFLAT,
            QUARTERFLAT, QUARTERSHARP, THREEQUARTERSFLAT, THREEQUARTERSSHARP, SHARPDOWN, SHARPUP,
            NATURALDOWN, NATURALUP, FLATDOWN, FLATUP, DOUBLESHARPDOWN, DOUBLESHARPUP, FLATFLATDOWN, FLATFLATUP,
            ARROWDOWN, ARROWUP, TRIPLESHARP, TRIPLEFLAT, SLASHQUARTERSHARP, SLASHSHARP, SLASHFLAT, DOUBLESLASHFLAT,
            SHARP1, SHARP2, SHARP3, SHARP5, FLAT1, FLAT2, FLAT3, FLAT4, SORI, KORON, OTHER
        };
        static String[] sAccidentalValue = { "sharp","natural","flat","double-sharp","sharp-sharp","flat-flat","natural-sharp","natural-flat",
                          "quarter-flat","quarter-sharp","three-quarters-flat","three-quarters-sharp","sharp-down",
                          "sharp-up","natural-down","natural-up","flat-down","flat-up","double-sharp-down","double-sharp-up",
                          "flat-flat-down","flat-flat-up","arrow-down","arrow-up","triple-sharp","triple-flat","slash-quarter-sharp",
                          "slash-sharp","slash-flat","double-slash-flat","sharp-1","sharp-2","sharp-3","sharp-5",
                          "flat-1","flat-2","flat-3","flat-4","sori","koron","other"};

        enum ArrowDirection
        {
            LEFT, UP, RIGHT, DOWN, NORTHWEST, NORTHEAST, SOUTHEAST, SOUTHWEST,
            LEFTRIGHT, UPDOWN, NORTHWESTSOUTHEAST, NORTHEASTSOUTHWEST, OTHER
        };
        static String[] sArrowDirection = { "left","up","right","down","northwest","northeast","southeast","southwest",
                          "left right","up down","northwest southeast","northeast southwest","other"};

        enum ArrowStyle { SINGLE, DOUBLE, FILLED, HOLLOW, PAIRED, COMBINED, OTHER };
        static String[] sArrowStyle = { "single", "double", "filled", "hollow", "paired", "combined", "other" };

        enum BackwardForward { BACKWARD, FORWARD };
        static String[] sBackwardForward = { "backward", "forward" };

        enum BarStyle { REGULAR, DOTTED, DASHED, HEAVY, LIGHTLIGHT, LIGHTHEAVY, HEAVYLIGHT, HEAVYHEAVY, TICK, SHORT, NONE };
        static String[] sBarStyle = { "regular","dotted","dashed","heavy","light-light",
                                "light-heavy","heavy-light","heavy-heavy","tick","short","none"};

        enum BeamValue { BEGIN, CONTINUE, END, FORWARDHOOK, BACKWARDHOOK };
        static String[] sBeamValue = { "begin", "continue", "end", "forward hook", "backward hook" };

        enum BeaterValue
        {
            BOW, CHIMEHAMMER, COIN, DRUMSTICK, FINGER, FINGERNAIL, FIST, GUIROSCRAPER, HAMMER, HAND, JAZZSTICK, KNITTINGNEEDLE,
            METALHAMMER, SLIDEBRUSHONGONG, SNARESTICK, SPOONMALLET, SUPERBALL, TRIANGLEBEATER, TRIANGLEBEATERPLAIN, WIREBRUSH
        };
        static String[] sBeaterValue = { "bow","chime hammer","coin","drum stick","finger","fingernail","fist","guiro scraper","hammer",
                                  "hand","jazz stick","knitting needle","metal hammer","slide brush on gong","snare stick",
                                  "spoon mallet","superball","triangle beater","triangle beater plain","wire brush"};

        enum BreathMarkValue { NONE, COMMA, TICK, UPBOW, SALZEDO };
        static String[] sBreathMarkValue = { "", "comma", "tick", "upbow", "salzedo" };

        enum CaesuraValue { NORMAL, THICK, SHORT, CURVED, SINGLE, NONE };
        static String[] sCaesuraValue = { "normal", "thick", "short", "curved", "single", "" };

        enum CancelLocation { LEFT, RIGHT, BEFOREBARLINE };
        static String[] sCancelLocation = { "left", "right", "before-barline" };

        enum CircularArrow { CLOCKWISE, ANTICLOCKWISE };
        static String[] sCircularArrow = { "clockwise", "anticlockwise" };

        enum ClefSign { G, F, C, PERCUSSION, TAB, JIANPU, NONE };
        static String[] sClefSign = { "G", "F", "C", "percussion", "TAB", "jianpu", "none" };

        enum CssFontSize { XXSMALL, XSMALL, SMALL, MEDIUM, LARGE, XLARGE, XXLARGE };
        static String[] sCssFontSize = { "xx-small", "x-small", "small", "medium", "large", "x-large", "xx-large" };

        enum DegreeSymbolValue { MAJOR, MINOR, AUGMENTED, DIMINISHED, HALFDIMINISHED };
        static String[] sDegreeSymbolValue = { "major", "minor", "augmented", "diminished", "half-diminished" };

        enum DegreeTypeValue { ADD, ALTER, SUBTRACT };
        static String[] sDegreeTypeValue = { "add", "alter", "subtract" };

        enum Effect
        {
            ANVIL, AUTOHORN, BIRDWHISTLE, CANNON, DUCKCALL, GUNSHOT, KLAXONHORN, LIONSROAR, LOTUSFLUTE, MEGAPHONE,
            POLICEWHISTLE, SIREN, SLIDEWHISTLE, THUNDERSHEET, WINDMACHINE, WINDWHISTLE
        };
        static String[] sEffect = { "anvil","auto horn","bird whistle","cannon","duck call","gun shot","klaxon horn","lions roar","lotus flute",
                            "megaphone","police whistle","siren","slide whistle","thunder sheet","wind machine","wind whistle"};

        enum EnclosureShape
        {
            RECTANGLE, SQUARE, OVAL, CIRCLE, BRACKET, TRIANGLE, DIAMOND, PENTAGON, HEXAGON, HEPTAGON,
            OCTAGON, NONAGON, DECAGON, NONE
        };
        static String[] sEnclosureShape = { "rectangle","square","oval","circle","bracket","triangle","diamond",
                                     "pentagon","hexagon","heptagon","octagon","nonagon","decagon","none"};

        enum Fan { ACCEL, RIT, NONE };
        static String[] sFan = { "accel", "rit", "none" };

        enum FermataShape { NORMAL, ANGLED, SQUARE, DOUBLEANGLED, DOUBLESQUARE, DOUBLEDOT, HALFCURVE, CURLEW, NONE };
        static String[] sFermataShape = {"normal","angled","square","double-angled","double-square",
                                  "double-dot","half-curve","curlew",""};

        enum FontStyle { NORMAL, ITALIC };
        static String[] sFontStyle = { "normal", "italic" };

        enum FontWeight { NORMAL, BOLD };
        static String[] sFontWeight = { "normal", "bold" };

        enum GlassValue { GLASSHARMONICA, GLASSHARP, WINDCHIMES };
        static String[] sGlassValue = { "glass harmonica", "glass harp", "wind chimes" };

        enum GroupBarlineValue { YES, NO, MENSURSTRICH };
        static String[] sGroupBarlineValue = { "yes", "no", "Mensurstrich" };

        enum GroupSymbolValue { NONE, BRACE, LINE, BRACKET, SQUARE };
        static String[] sGroupSymbolValue = { "none", "brace", "line", "bracket", "square" };

        enum HandbellValue
        {
            BELLTREE, DAMP, ECHO, GYRO, HANDMARTELLATO,
            MALLETLIFT, MALLETTABLE, MARTELLATO, MARTELLATOLIFT, MUTEDMARTELLATO, PLUCKLIFT, SWING
        };
        static String[] sHandbellValue = { "belltree","damp","echo","gyro","hand martellato","mallet lift","mallet table","martellato",
                                    "martellato lift","muted martellato","pluck lift","swing"};

        enum HarmonClosedLocation { RIGHT, BOTTOM, LEFT, TOP };
        static String[] sHarmonClosedLocation = { "right", "bottom", "left", "top" };

        enum HarmonClosedValue { YES, NO, HALF };
        static String[] sHarmonClosedValue = { "yes", "no", "half" };

        enum HarmonyType { EXPLICIT, IMPLIED, ALTERNATE };
        static String[] sHarmonyType = { "explicit", "implied", "alternate" };

        enum HoleClosedLocation { RIGHT, BOTTOM, LEFT, TOP };
        static String[] sHoleClosedLocation = { "right", "bottom", "left", "top" };

        enum HoleClosedValue { YES, NO, HALF };
        static String[] sHoleClosedValue = { "yes", "no", "half" };

        enum KindValue
        {
            MAJOR, MINOR, AUGMENTED, DIMINISHED, DOMINANT, MAJORSEVENTH, MINORSEVENTH, DIMINISHEDSEVENTH,
            AUGMENTEDSEVENTH, HALFDIMINISHED, MAJORMINOR, MAJORSIXTH, MINORSIXTH, DOMINANTNINTH, MAJORNINTH, MINORNINTH, DOMINANT11TH,
            MAJOR11TH, MINOR11TH, DOMINANT13TH, MAJOR13TH, MINOR13TH, SUSPENDEDSECOND, SUSPENDEDFOURTH, NEAPOLITAN, ITALIAN,
            FRENCH, GERMAN, PEDAL, POWER, TRISTAN, OTHER, NONE
        };
        static String[] sKindValue = { "major","minor","augmented","diminished","dominant","major-seventh","minor-seventh","diminished-seventh",
                          "augmented-seventh","half-diminished","major-minor","major-sixth","minor-sixth","dominant-ninth",
                          "major-ninth","minor-ninth","dominant-11th","major-11th","minor-11th","dominant-13th","major-13th",
                          "minor-13th","suspended-second","suspended-fourth","Neapolitan","Italian","French","German","pedal",
                          "power","Tristan","other","none"};

        enum LeftCenterRight { LEFT, CENTER, RIGHT };
        static String[] sLeftCenterRight = { "left", "center", "right" };

        enum LeftRight { LEFT, RIGHT };
        static String[] sLeftRight = { "left", "right" };

        enum LineEnd { UP, DOWN, BOTH, ARROW, NONE };
        static String[] sLineEnd = { "up", "down", "both", "arrow", "none" };

        enum LineLength { SHORT, MEDIUM, LONG };
        static String[] sLineLength = { "short", "medium", "long" };

        enum LineShape { STRAIGHT, CURVED };
        static String[] sLineShape = { "straight", "curved" };

        enum LineType { SOLID, DASHED, DOTTED, WAVY };
        static String[] sLineType = { "solid", "dashed", "dotted", "wavy" };

        enum MarginType { ODD, EVEN, BOTH };
        static String[] sMarginType = { "odd", "even", "both" };

        enum MeasureNumberingValue { NONE, MEASURE, SYSTEM };
        static String[] sMeasureNumberingValue = { "none", "measure", "system" };

        enum Membrane
        {
            BASSDRUM, BASSDRUMONSIDE, BONGOS, CHINESETOMTOM, CONGADRUM, CUICA, GOBLETDRUM, INDOAMERICANTOMTOM,
            JAPANESETOMTOM, MILITARYDRUM, SNAREDRUM, SNAREDRUMSNARESOFF, TABLA, TAMBOURINE, TENORDRUM, TIMBALES, TOMTOM
        };
        static String[] sMembrane = { "bass drum","bass drum on side","bongos","Chinese tomtom","conga drum","cuica",
                               "goblet drum","Indo-American tomtom","Japanese tomtom","military drum","snare drum",
                               "snare drum snares off","tabla","tambourine","tenor drum","timbales","tomtom"};

        enum Metal
        {
            AGOGO, ALMGLOCKEN, BELL, BELLPLATE, BELLTREE, BRAKEDRUM, CENCERRO, CHAINRATTLE,
            CHINESECYMBAL, COWBELL, CRASHCYMBALS, CROTALE, CYMBALTONGS, DOMEDGONG, FINGERCYMBALS, FLEXATONE, GONG, HIHAT,
            HIGHHATCYMBALS, HANDBELL, JAWHARP, JINGLEBELLS, MUSICALSAW, SHELLBELLS, SISTRUM, SIZZLECYMBAL,
            SLEIGHBELLS, SUSPENDEDCYMBAL, TAMTAM, TAMTAMWITHBEATER, TRIANGLE, VIETNAMESEHAT
        };
        static String[] sMetal = {"agogo","almglocken","bell","bell plate","bell tree","brake drum","cencerro","chain rattle",
                         "Chinese cymbal","cowbell","crash cymbals","crotale","cymbal tongs","domed gong","finger cymbals",
                         "flexatone","gong","hi-hat","high-hat cymbals","handbell","jaw harp","jingle bells","musical saw",
                         "shell bells","sistrum","sizzle cymbal","sleigh bells","suspended cymbal","tam tam",
                         "tam tam with beater","triangle","Vietnamese hat"};

        enum Mute
        {
            ON, OFF, STRAIGHT, CUP, HARMONNOSTEM, HARMONSTEM, BUCKET, PLUNGER, HAT,
            SOLOTONE, PRACTICE, STOPMUTE, STOPHAND, ECHO, PALM
        };
        static String[] sMute = { "on","off","straight","cup","harmon-no-stem","harmon-stem","bucket",
                        "plunger","hat","solotone","practice","stop-mute","stop-hand","echo","palm"};

        enum NoteSizeType { CUE, GRACE, GRACECUE, LARGE };
        static String[] sNoteSizeType = { "cue", "grace", "grace-cue", "large" };

        enum NoteTypeValue
        {
            x1024TH, x512TH, x256TH, x128TH, x64TH, x32ND, x16TH, EIGHTH, QUARTER, HALF, WHOLE, BREVE, LONG, MAXIMA
        };
        static String[] sNoteTypeValue = {"1024th","512th","256th","128th","64th","32nd","16th",
                                      "eighth","quarter","half","whole","breve","long","maxima"};

        enum NoteheadValue
        {
            SLASH, TRIANGLE, DIAMOND, SQUARE, CROSS, X, CIRCLEX, INVERTEDTRIANGLE,
            ARROWDOWN, ARROWUP, CIRCLED, SLASHED, BACKSLASHED, NORMAL, CLUSTER, CIRCLEDOT, LEFTTRIANGLE, RECTANGLE,
            NONE, DO, RE, MI, FA, FAUP, SO, LA, TI, OTHER
        };
        static String[] sNoteheadValue = { "slash","triangle","diamond","square","cross","x","circle-x","inverted triangle","arrow down",
                                    "arrow up","circled","slashed","back slashed","normal","cluster","circle dot","left triangle",
                                    "rectangle","none","do","re","mi","fa","fa up","so","la","ti","other"};

        enum OverUnder { OVER, UNDER };
        static String[] sOverUnder = { "over", "under" };

        enum PedalType { START, STOP, SOSTENUTO, CHANGE, CONTINUE };
        static String[] sPedalType = { "start", "stop", "sostenuto", "change", "continue" };

        enum PitchedValue
        {
            CELESTA, CHIMES, GLOCKENSPIEL, LITHOPHONE, MALLET, MARIMBA, STEELDRUMS, TUBAPHONE,
            TUBULARCHIMES, VIBRAPHONE, XYLOPHONE
        };
        static String[] sPitchedValue = { "celesta","chimes","glockenspiel","lithophone","mallet","marimba","steel drums",
                          "tubaphone","tubular chimes","vibraphone","xylophone"};

        enum PrincipalVoiceSymbol { HAUPTSTIMME, NEBENSTIMME, PLAIN, NONE };
        static String[] sPrincipalVoiceSymbol = { "Hauptstimme", "Nebenstimme", "plain", "none" };

        enum RightLeftMiddle { RIGHT, LEFT, MIDDLE };
        static String[] sRightLeftMiddle = { "right", "left", "middle" };

        enum SemiPitched { HIGH, MEDIUMHIGH, MEDIUM, MEDIUMLOW, LOW, VERYLOW };
        static String[] sSemiPitched = { "high", "medium-high", "medium", "medium-low", "low", "very-low" };

        enum ShowFrets { NUMBERS, LETTERS };
        static String[] sShowFrets = { "numbers", "letters" };

        enum ShowTuplet { ACTUAL, BOTH, NONE };
        static String[] sShowTuplet = { "actual", "both", "none" };

        enum StaffDivideSymbol { DOWN, UP, UPDOWN };
        static String[] sStaffDivideSymbol = { "down", "up", "up-down" };

        enum StaffType { OSSIA, CUE, EDITORIAL, REGULAR, ALTERNATE };
        static String[] sStaffType = { "ossia", "cue", "editorial", "regular", "alternate" };

        enum StartNote { UPPER, MAIN, BELOW };
        static String[] sStartNote = { "upper", "main", "below" };

        enum StartStop { START, STOP };
        static String[] sStartStop = { "start", "stop" };

        enum StartStopChangeContinue { START, STOP, CHANGE, CONTINUE };
        static String[] sStartStopChangeContinue = { "start", "stop", "change", "continue" };

        enum StartStopContinue { START, STOP, CONTINUE };
        static String[] sStartStopContinue = { "start", "stop", "continue" };

        enum StartStopDiscontinue { START, STOP, DISCONTINUE };
        static String[] sStartStopDiscontinue = { "start", "stop", "discontinue" };

        enum StartStopSingle { START, STOP, SINGLE };
        static String[] sStartStopSingle = { "start", "stop", "single" };

        enum StemValue { DOWN, UP, DOUBLE, NONE };
        static String[] sStemValue = { "down", "up", "double", "none" };

        enum Step { A, B, C, D, E, F, G };
        static String[] sStep = { "A", "B", "C", "D", "E", "F", "G" };

        enum StickLocation { CENTER, RIM, CYMBALBELL, CYMBALEDGE };
        static String[] sStickLocation = { "center", "rim", "cymbal bell", "cymbal edge" };

        enum StickMaterial { SOFT, MEDIUM, HARD, SHADED, X };
        static String[] sStickMaterial = { "soft", "medium", "hard", "shaded", "x" };

        enum StickType { BASSDRUM, DOUBLEBASSDRUM, GLOCKENSPIEL, GUM, HAMMER, SUPERBALL, TIMPANI, WOUND, XYLOPHONE, YARN };
        static String[] sStickType = {"bass drum","double bass drum","glockenspiel","gum","hammer","superball",
                               "timpani","wound","xylophone","yarn"};

        enum Syllabic { SINGLE, BEGIN, END, MIDDLE };
        static String[] sSyllabic = { "single", "begin", "end", "middle" };

        enum SymbolSize { FULL, F, GRACECUE, LARGE };
        static String[] sSymbolSize = { "full", "F", "grace-cue", "large" };

        enum TapHand { LEFT, RIGHT };
        static String[] sTapHand = { "left", "right" };

        enum TextDirection { LTR, RTL, LRO, RLO };
        static String[] sTextDirection = { "ltr", "rtl", "lro", "rlo" };

        enum TiedType { START, STOP, CONTINUE, LETRING };
        static String[] sTiedType = { "start", "stop", "continue", "let-ring" };

        enum TimeRelation { PARENTHESES, BRACKET, EQUALS, SLASH, SPACE, HYPHEN };
        static String[] sTimeRelation = { "parentheses", "bracket", "equals", "slash", "space", "hyphen" };

        enum TimeSeparator { NONE, HORIZONTAL, DIAGONAL, VERTICAL, ADJACENT };
        static String[] sTimeSeparator = { "none", "horizontal", "diagonal", "vertical", "adjacent" };

        enum TimeSymbol { COMMON, CUT, SINGLENUMBER, NOTE, DOTTEDNOTE, NORMAL };
        static String[] sTimeSymbol = { "common", "cut", "single-number", "note", "dotted-note", "normal" };

        enum TipDirection { UP, DOWN, LEFT, RIGHT, NORTHWEST, NORTHEAST, SOUTHEAST, SOUTHWEST };
        static String[] sTipDirection = { "up", "down", "left", "right", "northwest", "northeast", "southeast", "southwest" };

        enum TopBottom { TOP, BOTTOM };
        static String[] sTopBottom = { "top", "bottom" };

        enum TremoloType { START, STOP, SINGLE, UNMEASURED };
        static String[] sTremoloType = { "start", "stop", "single", "unmeasured" };

        enum TrillStep { WHOLE, HALF, UNISON };
        static String[] sTrillStep = { "whole", "half", "unison" };

        enum TwoNoteTurn { WHOLE, HALF, NONE };
        static String[] sTwoNoteTurn = { "whole", "half", "none" };

        enum UpDownStopContinue { UP, DOWN, STOP, CONTINUE };
        static String[] sUpDownStopContinue = { "up", "down", "stop", "continue" };

        enum UpDown { UP, DOWN };
        static String[] sUpDown = { "up", "down" };

        enum UprightInverted { UPRIGHT, INVERTED };
        static String[] sUprightInverted = { "upright", "inverted" };

        enum VAlign { TOP, MIDDLE, BOTTOM, BASELINE };
        static String[] sVAlign = { "top", "middle", "bottom", "baseline" };

        enum VAlignImage { TOP, MIDDLE, BOTTOM };
        static String[] sVAlignImage = { "top", "middle", "bottom" };

        enum WedgeType { CRESCENDO, DIMINUENDO, STOP, CONTINUE };
        static String[] sWedgeType = { "crescendo", "diminuendo", "stop", "continue" };

        enum Winged { NONE, STRAIGHT, CURVED, DOUBLESTRAIGHT, DOUBLECURVED };
        static String[] sWinged = { "none", "straight", "curved", "double-straight", "double-curved" };

        enum Wood
        {
            BAMBOOSCRAPER, BOARDCLAPPER, CABASA, CASTANETS, CASTANETSWITHHANDLE, CLAVES, FOOTBALLRATTLE,
            GUIRO, LOGDRUM, MARACA, MARACAS, QUIJADA, RAINSTICK, RATCHET, RECORECO, SANDPAPERBLOCKS, SLITDRUM, TEMPLEBLOCK,
            VIBRASLAP, WHIP, WOODBLOCK
        };
        static String[] sWood = { "bamboo scraper","board clapper","cabasa","castanets","castanets with handle","claves",
                          "football rattle","guiro","log drum","maraca","maracas","quijada","rainstick","ratchet",
                          "reco-reco","sandpaper blocks","slit drum","temple block","vibraslap","whip","wood block"};
    }
}
