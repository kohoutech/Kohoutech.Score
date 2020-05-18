/* ----------------------------------------------------------------------------
Kohoutech Score Library
Copyright (C) 1997-2020  George E Greaney

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

namespace Kohoutech.Score.Symbols
{
    class XmlClasses
    {
    }

    public class Accidental	{}
    public class AccidentalMark 	    {}
    public class AccidentalText		{}
    public class Accord			{}
    public class AccordionRegistration  {}
    public class Appearance		{}
    public class Arpeggiate		{}
    public class Arrow			{}
    public class Articulations		{}
    public class BarStyleColor		{}
    public class Barline			{}
    public class Barre			{}
    public class Bass			{}
    public class BassAlter		{}
    public class BassStep		{}
    public class Beam			{}
    public class BeatUnitTied		{}
    public class Beater			{}
    public class Bend			{}
    public class Bookmark		{}
    public class Bracket			{}
    public class BreathMark		{}
    public class Caesura			{}
    public class Coda			{}
    public class Credit			{}
    public class Dashes			{}
    public class Defaults		{}
    public class Degree			{}
    public class DegreeAlter		{}
    public class DegreeType		{}
    public class DegreeValue		{}
    public class Direction : Symbol		{}
    public class Directiontype		{}
    public class Distance		{}
    public class Dynamics		{}
    public class Elision			{}
    public class EmptyFont		{}
    public class EmptyLine		{}
    public class EmptyPlacementSmufl	{}
    public class EmptyPlacement			{}
    public class EmptyPrintObjectStyleAlign	{}
    public class EmptyPrintStyleAlignId	{}
    public class EmptyTrillSound	{}
    public class Encoding		{}
    public class Ending			{}
    public class Extend			{}
    public class Feature			{}
    public class Fermata			{}
    public class Figure			{}
    public class FiguredBass		{}
    public class Fingering		{}
    public class FirstFret		{}
    public class FormattedSymbolId	{}
    public class FormattedTextId	{}
    public class FormattedText		{}
    public class FrameNote		{}
    public class Frame			{}
    public class Fret			{}
    public class Glass			{}
    public class Glissando		{}
    public class Glyph			{}
    public class Grace			{}
    public class GroupBarline		{}
    public class GroupName		{}
    public class GroupSymbol		{}
    public class Grouping		{}
    public class HammerOnPullOff	{}
    public class Handbell		{}
    public class HarmonClosed		{}
    public class HarmonMute		{}
    public class Harmonic		{}
    public class Harmony			{}
    public class HarpPedals		{}
    public class HeelToe		{}
    public class Hole			{}
    public class HoleClosed		{}
    public class HorizontalTurn		{}
    public class Identification		{}
    public class Image			{}
    public class Instrument		{}
    public class Inversion		{}
    public class Kind			{}
    public class Level			{}
    public class LineWidth		{}
    public class Link			{}
    public class Lyric			{}
    public class LyricFont		{}
    public class LyricLanguage		{}
    public class MeasureLayout		{}
    public class MeasureNumbering	{}
    public class Metronome		{}
    public class MetronomeBeam		{}
    public class MetronomeNote		{}
    public class MetronomeTied		{}
    public class MetronomeTuplet	{}
    public class MidiDevice		{}
    public class MidiInstrument		{}
    public class Miscellaneous		{}
    public class MiscellaneousField	{}
    public class Mordent			{}
    public class NameDisplay		{}
    public class NonArpeggiate		{}
    public class Notations		{}
    //public class Note			{}
    public class NoteSize		{}
    public class NoteType		{}
    public class Notehead		{}
    public class NoteheadText		{}
    public class OctaveShift		{}
    public class Offset			{}
    public class Opus			{}
    public class Ornaments		{}
    public class OtherAppearance	{}
    public class OtherDirection		{}
    public class OtherNotation		{}
    public class OtherPlacementText	{}
    public class OtherPlay		{}
    public class OtherText		{}
    public class PageLayout		{}
    public class PageMargins		{}
    public class PartGroup		{}
    public class PartList		{}
    public class PartName		{}
    public class Pedal			{}
    public class PedalTuning		{}
    public class PerMinute		{}
    public class Percussion		{}
    public class Pitch			{}
    public class Pitched			{}
    public class PlacementText		{}
    public class Play			{}
    public class PrincipalVoice	{}
    public class Print			{}
    public class Repeat			{}
    public class Rest			{}
    public class Root			{}
    public class RootAlter		{}
    public class RootStep		{}
    public class Scaling			{}
    public class Scordatura		{}
    public class ScoreInstrument	{}
    public class ScorePart		{}
    public class Segno			{}
    public class Slide			{}
    public class Slur			{}
    public class Sound			{}
    public class StaffDivide		{}
    public class StaffLayout		{}
    public class Stem			{}
    public class Stick			{}
    public class StringSym			{}
    public class StringMute		{}
    public class StrongAccent		{}
    public class StyleText		{}
    public class Supports		{}
    public class SystemDividers	{}
    public class SystemLayout		{}
    public class SystemMargins		{}
    public class Tap			{}
    public class Technical		{}
    public class TextElementData	{}
    public class Tie			{}
    public class Tied			{}
    public class TimeModification	{}
    public class Tremolo			{}
    public class Tuplet			{}
    public class TupletDot		{}
    public class TupletNumber		{}
    public class TupletPortion		{}
    public class TupletType		{}
    public class TypedText		{}
    public class Unpitched		{}
    public class VirtualInstrument	{}
    public class WavyLine		{}
    public class Wedge			{}
    public class Work			{}

//- element groups ------------------------------------------------------------

    public class AllMargins { }
    public class BeatUnit { }
    public class DisplayStepOctave { }
    public class Duration { }
    public class Editorial { }
    public class EditorialVoice { }
    public class EditorialVoiceDirection { }
    public class Footnote { }
    public class FullNote { }
    public class HarmonyChord { }
    public class Layout { }
    public class LeftRightMargins { }
    public class LevelGroup { }
    public class MusicData { }
    public class NonTraditionalKey { }
    public class PartGroupGroup { }
    public class ScorePartGroup { }
    public class SlashGroup { }
    public class StaffX { }
    public class TimeSignatureGroup { }
    public class TraditionalKey { }
    public class Tuning { }
    public class Voice { }
}
