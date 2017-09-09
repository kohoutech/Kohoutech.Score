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

using Transonic.Score;

namespace Transonic.Score.Symbols
{
    public class Note : Symbol
    {
        public const int quantization = 8;         //quantize notes to 1/32 note pos (quarter note / 8)

        public const String flat = "\u266d";
        public const String natural = "\u266e";
        public const String sharp = "\u266f";

        //public const String quarter = "\u2669";

        public int noteNumber;          //midi pitch
        public int octave;
        public int step;
        public int ledgerLinesAbove;
        public bool ledgerLinesMiddle;
        public int ledgerLinesBelow;
        public bool hasSharp;

        public Note(int _start, int _noteNum, int _dur)
        {
            startTick = _start;
            noteNumber = _noteNum;
            duration = _dur;

            octave = noteNumber / 12;
            step = noteNumber % 12;
            ledgerLinesAbove = 0;
            ledgerLinesMiddle = false;
            ledgerLinesBelow = 0;
        }

        public override void setMeasure(Measure measure)
        {
            base.setMeasure(measure);

            startTick -= measure.startTick;

            //quantize start to nearest beat
            double val = (double)startTick / measure.staff.division;
            beat = (int)((val * quantization) + 0.5f);

            //quatize duration to next beat
            val = (double)duration / measure.staff.division;
            len = (int) Math.Ceiling((val * quantization * 2));

            setVertPos();
            hasSharp = (keyOfC[step] == 1);
        }

        int[] scaleTones = { 0, 0, 1, 1, 2, 3, 3, 4, 4, 5, 5, 6 };
        int[] keyOfC = { 0, 1, 0, 1, 0, 0, 1, 0, 1, 0, 1, 0 };

        //set verical note pos relative to top of staff
        //notes from middle C and higher belong to the treble clef, notes below middle C belong to bass clef
        public void setVertPos()
        { 
            int halfStep = Staff.lineSpacing / 2;

            //treble clef
            if (noteNumber >= 60)
            {
                int cpos = Staff.lineSpacing * 5;        //pos of middle C
                ypos = cpos - (((octave - 5) * halfStep * 7) + (scaleTones[step] * halfStep));
                if (ypos < 0)
                {
                    ledgerLinesAbove = ypos / Staff.lineSpacing;
                }
                ledgerLinesMiddle |= (noteNumber == 60 | noteNumber == 61);

            }

            //bass clef
            else
            {
                int cpos = Staff.grandHeight + Staff.lineSpacing * 12 + halfStep;    //pos of MIDI C = 0
                ypos = cpos - ((octave * halfStep * 7) + (scaleTones[step] * halfStep));
                if (ypos > Staff.grandHeight)
                {
                    ledgerLinesBelow = (ypos - Staff.grandHeight) / Staff.lineSpacing;
                }
            }
        }

        public override void dump()
        {
            float tick = (float)startTick / measure.staff.division;
            float dur = (float)duration / measure.staff.division;

            Console.WriteLine("Measure " + measure.number + " note: " + noteNumber +
                " at " + beat.ToString("F2") + "(" + tick.ToString("F2") +
                ") len " + len.ToString("F2") + "(" + dur.ToString("F2") + ")");
        }

//- display -------------------------------------------------------------------

        public override void paint(Graphics g, int xorg, int top)
        {
            xorg += xpos;
            if (ledgerLinesAbove < 0)
            {
                int linepos = top - Staff.lineSpacing;
                for (int i = 0; i > ledgerLinesAbove; i--)
                {
                    g.DrawLine(Pens.Red, xorg - 2, linepos, xorg + 10, linepos);
                    linepos -= Staff.lineSpacing;
                }
            }

            if (ledgerLinesMiddle)
            {
                g.DrawLine(Pens.Red, xorg - 2, top + (Staff.lineSpacing * 5), xorg + 10, top + (Staff.lineSpacing * 5));
            }

            if (ledgerLinesBelow > 0)
            {
                int linepos = top + Staff.grandHeight + Staff.lineSpacing;
                for (int i = 0; i < ledgerLinesBelow; i++)
                {
                    g.DrawLine(Pens.Red, xorg - 2, linepos, xorg + 10, linepos);
                    linepos += Staff.lineSpacing;
                }
            }

            top += ypos;

            if (hasSharp)
            {
                Font sharpfont = new Font("Arial", 14);
                g.DrawString(sharp, sharpfont, Brushes.Red, xorg - 12, top - 12);

            }

            g.FillEllipse(Brushes.Red, xorg, top - 4, 8, 8);
            g.DrawLine(Pens.Red, xorg + 8, top, xorg + 8, top - Staff.lineSpacing * 3);

        }
    }
}
