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
using System.Drawing;
using System.Drawing.Drawing2D;

namespace Transonic.Score
{
    public class Symbol
    {
        public Measure measure;
        public float start;             //symbol start in measure in fractions of a beat
        public float len;               //symbol len in fractions of a beat
        public int pos;                 //symbol pos in measure in pixels

        public Symbol()
        {
        }

        public virtual void setMeasure(Measure _measure)
        {
            measure = _measure;
        }

        public virtual void dump()
        {
        }

        public virtual void paint(Graphics g, int xpos, int ypos)
        {
        }
    }

//-----------------------------------------------------------------------------

    public class TimeSignature : Symbol
    {
        public TimeSignature()            
        {
        }
    }

//-----------------------------------------------------------------------------

    public class KeySignature : Symbol
    {
        public KeySignature()
        {
        }
    }

//-----------------------------------------------------------------------------

    public class Note : Symbol
    {
        public const int quantization = 4;         //quantize notes to 1/32 note pos (quarter note / 8)

        public const String flat = "\u266d";
        public const String natural = "\u266e";
        public const String sharp = "\u266f";

        //public const String quarter = "\u2669";

        public int noteNumber;          //midi pitch
        public int startTick;           //start tick relative to measure start
        public int duration;            //length in ticks
        public int octave;
        public int step;

        public Note(int _start, int _noteNum, int _dur)
        {
            startTick = _start;
            noteNumber = _noteNum;
            duration = _dur;
            octave = noteNumber / 12;
            step = noteNumber % 12;
        }

        public override void setMeasure(Measure measure)
        {
            base.setMeasure(measure);

            startTick -= measure.startTime;

            //quantize val to nearest beat fraction
            float val = (float)startTick / measure.staff.division;
            int roundoff = (int)((val * quantization) + 0.5f);
            start = ((float)roundoff) / quantization;

            //quatize duration to next beat fraction
            val = (float)duration / measure.staff.division;
            roundoff = (int)((val * quantization) + 1.0f);
            len = ((float)roundoff) / quantization;
        }

        public override void dump()
        {
            float tick = (float)startTick / measure.staff.division;
            float dur = (float)duration / measure.staff.division;

            Console.WriteLine("Measure " + measure.number + " note: " + noteNumber +
                " at " + start.ToString("F2") + "(" + tick.ToString("F2") + 
                ") len " + len.ToString("F2") + "(" + dur.ToString("F2") + ")");
        }

        int[] scaleTones = { 0, 0, 1, 1, 2, 3, 3, 4, 4, 5, 5, 6 };
        int[] keyOfC =   { 0, 1, 0, 1, 0, 0, 1, 0, 1, 0, 1, 0 };

        public override void paint(Graphics g, int xpos, int ypos)
        {
            int halfStep = Staff.lineSpacing / 2;

            //treble clef
            if (noteNumber >= 60)
            {
                int cpos = ypos + Staff.lineSpacing * 5;        //pos of middle C
                int notepos = cpos - (((octave - 5) * halfStep * 7) + (scaleTones[step] * halfStep));
                if (notepos < (ypos - Staff.lineSpacing))
                {
                    int linepos = ypos - Staff.lineSpacing;
                    while (linepos >= notepos) {
                        g.DrawLine(Pens.Black, xpos + pos - 4, linepos, xpos + pos + 12, linepos);
                        linepos -= Staff.lineSpacing;
                    }
                }
                if ((noteNumber == 60) || (noteNumber == 61))
                {
                    g.DrawLine(Pens.Black, xpos + pos - 4, notepos, xpos + pos + 12, notepos);
                }
                g.FillEllipse(Brushes.Red, xpos + pos, notepos - 4, 8, 8);
            }

            //bass clef
            else
            {
                int cpos = ypos + Staff.grandHeight + Staff.lineSpacing * 12 + halfStep;    //pos of MIDI C = 0
                int noteofs = noteNumber;
                int octave = noteofs / 12;
                int step = noteofs % 12;
                int notepos = cpos - ((octave * halfStep * 7) + (scaleTones[step] * halfStep));
                if (notepos > (ypos + Staff.grandHeight))
                {
                    int linepos = ypos + Staff.grandHeight;
                    while (linepos <= notepos)
                    {
                        g.DrawLine(Pens.Black, xpos + pos - 4, linepos, xpos + pos + 12, linepos);
                        linepos += Staff.lineSpacing;
                    }
                }
                g.FillEllipse(Brushes.Red, xpos + pos, notepos - 4, 8, 8);
            }


            //String quart = char.ConvertFromUtf32(0x1d15f);
            //    Font notefont = new Font("Segoe UI Symbol", 48);
            //    g.DrawString(quart, notefont, Brushes.Blue, xpos, notepos);
            //    if (hassharp[step] == 1)
            //    {
            //        Font sharpfont = new Font("Arial", 16);
            //        g.DrawString(sharp, notefont, Brushes.Blue, xpos - 12, notepos - 12);
            //    }
        }
    }

//-----------------------------------------------------------------------------

    public class Rest : Symbol
    {
        public Rest(float _start, float _len)
        {
            start = _start;
            len = _len;
        }

        public override void paint(Graphics g, int xpos, int ypos)
        {
            g.FillRectangle(Brushes.Blue, xpos + pos, ypos + Staff.lineSpacing, 6, 3);

        }
    }

}
