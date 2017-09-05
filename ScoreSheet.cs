/* ----------------------------------------------------------------------------
Transonic Score Library
Copyright (C) 1997-2017  George E Greaney

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
using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Drawing2D;

using Transonic.MIDI;

namespace Transonic.Score
{
    public class ScoreSheet : UserControl
    {
        IScoreWindow window;
        Sequence seq;
        List<Staff> staves;
        Staff displayStaff;
        int curTick;

        public ScoreSheet(IScoreWindow _window)
        {
            window = _window;
            seq = null;
            InitializeComponent();
            staves = new List<Staff>();
            displayStaff = null;
            curTick = 0;
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // ScoreSheet
            // 
            this.BackColor = System.Drawing.Color.LightGoldenrodYellow;
            this.Name = "ScoreSheet";
            this.Size = new System.Drawing.Size(650, 212);
            this.ResumeLayout(false);

        }

//- sequencing ----------------------------------------------------------------

        public void setSequence(Sequence _seq)
        {
            seq = _seq;
            parseSequence();
        }

        private void parseSequence()
        {
            staves.Clear();
            for (int i = 1; i < seq.tracks.Count; i++)
            {
                Staff staff = parseTrack(seq.tracks[i], i);
                staves.Add(staff);
            }
        }

        private Staff parseTrack(Track track, int staffNum)
        {
            Staff staff = new Staff(this, staffNum, seq.division);
            List<Symbol> syms = getTrackSymbols(track);                //get all the notes, time & key sigs for this track
            buildStaff(staff, syms);                                   //create measures for staff & put symbols in them
            
            foreach (Measure measure in staff.measures)
            {
                measure.layoutSymbols();
            }

            //staff.dump();
            return staff;
        }

        //scan track, convert note on/note off pairs into list of notes for single track
        //if we can't find a note off for a note on, we ignore it (stuck note?)
        //add in time and key signature symbols
        public List<Symbol> getTrackSymbols(Track track)
        {
            List<Symbol> notes = new List<Symbol>();
            for (int i = 0; i < track.events.Count; i++)
            {
                Event evt = track.events[i];

                //note on/note off pair
                if (evt.msg is NoteOnMessage)
                {
                    NoteOnMessage noteOn = (NoteOnMessage)evt.msg;
                    for (int j = i + 1; j < track.events.Count; j++)
                    {
                        if (track.events[j].msg is NoteOffMessage)
                        {
                            NoteOffMessage noteOff = (NoteOffMessage)track.events[j].msg;
                            if ((noteOn.noteNumber == noteOff.noteNumber) && (noteOn.channel == noteOff.channel))
                            {
                                int duration = (int)(track.events[j].time - evt.time);
                                Note note = new Note((int)evt.time, noteOn.noteNumber, duration);
                                notes.Add(note);
                                break;
                            }
                        }
                    }
                }

                if (evt.msg is TimeSignatureMessage)
                {
                }

                if (evt.msg is KeySignatureMessage)
                {
                }

            }
            return notes;
        }

        public void buildStaff(Staff staff, List<Symbol> syms)
        {
            int ticksPerMeasure = seq.division * 4;         //for the moment assume 4/4 time
            int measureNum = 1;
            int measureTime = 0;
            Measure measure = new Measure(staff, measureNum, measureTime, ticksPerMeasure);        //initial measure
            staff.addMeasure(measure);

            foreach (Symbol sym in syms)
            {
                if (sym is Note)
                {
                    Note note = (Note)sym;
                    int noteTime = note.startTick;
                    int noteMeasure = (noteTime / ticksPerMeasure) + 1;
                    while (noteMeasure > measureNum)                        //add empty measures until we get to the one we're one now
                    {
                        measureTime += ticksPerMeasure;
                        measure = new Measure(staff, ++measureNum, measureTime, 4);
                        staff.addMeasure(measure);
                    }
                    measure.addSymbol(note);
                    note.setMeasure(measure);
                }
            }
        }

        public void setDisplayStaff(int staffNum)
        {
            if (staffNum < seq.tracks.Count)
            {
                displayStaff = staves[staffNum - 1];
                displayStaff.setCurrentMeasure(curTick);
                Invalidate();
            }
            else
            {
                displayStaff = null;
            }
        }

        public void setCurrentPos(int tick)
        {
            curTick = tick;
            if (displayStaff != null)
            {
                displayStaff.setCurrentMeasure(curTick);
                Invalidate();
            }
        }

//- painting ------------------------------------------------------------------

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            Graphics g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;

            if (displayStaff != null)
            {
                displayStaff.paint(g);
            }
        }
    }

//-----------------------------------------------------------------------------

    public interface IScoreWindow
    {
    }

}

//Console.WriteLine("there's no sun in the shadow of the wizard");
