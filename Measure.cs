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
    public class Measure
    {
        //dimensions based upon typograhy in Beethoven's Complete Piano Sonatas, Dover ed.
        public static int noteSpacing = 12;
        public static int minWidth = 48;
        public const int quantization = 4;         //quantize rests to 1/32 note pos (quarter note / 8)

        public Staff staff;
        List<Symbol> symbols;

        public int number;
        public int startTime;               //measure start time in ticks
        public int length;                  //number of beats in measure, determined by most recent key signature
        public TimeSignature timeSig;
        public KeySignature keySig;

        int width;

        public Measure(Staff _staff, int num, int start, int len)
        {
            staff = _staff;
            number = num;
            length = len;
            startTime = start;
            timeSig = null;
            keySig = null;

            symbols = new List<Symbol>();

            width = 50;
        }

        public void addSymbol(Symbol sym)
        {
            symbols.Add(sym);
        }

        public void dump()
        {
            foreach (Symbol sym in symbols)
            {
                sym.dump();
            }
        }

        //insert a rest at any point in the measure there isn't a note playing
        public void insertRests()
        {
            if (symbols.Count == 0)
            {
                Rest rest = new Rest(0, 4);
                symbols.Add(rest);
                rest.setMeasure(this);
            }
            else
            {
                List<Symbol> syms = new List<Symbol>();
                float beat = 0;
                for (int i = 0; i < symbols.Count; i++)
                {
                    Note note = (Note)symbols[i];
                    if (note.start > (beat + quantization))
                    {
                        float restLen = note.start - beat;
                        Rest rest = new Rest(beat, restLen);
                        syms.Add(rest);
                        rest.setMeasure(this);
                        beat = note.start + note.len;
                    }
                    syms.Add(note);
                }
                symbols = syms;
            }
        }

        public void layoutSymbols()
        {
            insertRests();
            int symPos = noteSpacing;
            float beat = 0;
            foreach (Symbol sym in symbols)
            {
                if (sym.start > beat)
                {
                    beat = sym.start;
                    symPos += noteSpacing;
                }
                sym.pos = symPos;
            }
            width = symPos + noteSpacing;
            if (width < minWidth) width = minWidth;
        }

        public int getWidth()
        {
            return width;
        }

        public void paint(Graphics g, int left, int top)
        {
            //measure num
            g.DrawString(number.ToString(), SystemFonts.DefaultFont, Brushes.Black, left, top - 14);

            //symbols
            for (int i = 0; i < symbols.Count; i++)
            {
                symbols[i].paint(g, left, top);
            }

            //barline
            g.DrawLine(Pens.Black, left + width, top, left + width, top + Staff.grandHeight);
        }
    }
}

//Console.WriteLine("there's no sun in the shadow of the wizard");
