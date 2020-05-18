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

using Kohoutech.MIDI;
using Kohoutech.Score.Symbols;

namespace Kohoutech.Score.Midi
{
    class ScoreMidi
    {
        public static int seqdiv = 120;

        public static Sequence ConvertScoreToMidi (ScoreDoc doc) 
        {
            Sequence seq = new Sequence(seqdiv);
            for (int i = 0; i < doc.parts.Count; i++)
            {
                Track track = getTrackFromPart(doc.parts[i]);
                seq.addTrack(track);
            }
            return seq;
        }

        public static Track getTrackFromPart(Part part)
        {
            Track track = new Track();
            track.name = part.id;
            for (int i = 0; i < part.staves.Count; i++)
            {
                Staff staff = part.staves[i];
                decimal measurepos = 0;
                for (int j = 0; j < staff.measures.Count; j++)
                {
                    getEventsFromMeasure(track, staff.measures[j], measurepos);
                    measurepos += staff.measures[j].length;
                }
            }
            return track;
        }

        private static void getEventsFromMeasure(Track track, Measure measure, decimal measurepos)
        {
            decimal beatTick = seqdiv / measure.attr.divisions;         //num of midi ticks per division                
            for (int i = 0; i < measure.beats.Count; i++)
            {
                Beat beat = measure.beats[i];
                decimal beatTime = beat.beatpos + measurepos;
                getEventsFromBeat(track, beat, beatTime, beatTick);
            }
        }

        private static void getEventsFromBeat(Track track, Beat beat, decimal beattime, decimal beattick)
        {
            foreach (Symbol sym in beat.symbols) {
                if (sym is Note)
                {
                    Note note = (Note)sym;
                    NoteOnMessage onmsg = new NoteOnMessage(0, note.notenum, 0x60);
                    uint evtime = (uint)(beattime * beattick);
                    Event evt = new MessageEvent(evtime, onmsg);
                    track.addEvent(evt);
                    NoteOffMessage msg = new NoteOffMessage(0, note.notenum, 0x60);
                    evtime += (uint)(note.duration * beattick);
                    evt = new MessageEvent(evtime, msg);
                    track.addEvent(evt);
                }
            }
        }
    }
}
