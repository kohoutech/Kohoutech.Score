/* ----------------------------------------------------------------------------
Transonic Score Library
Copyright (C) 1997-2018 George E Greaney

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

using Transonic.Score.Symbols;

namespace Transonic.Score
{
    public class Beat
    {
        public const decimal BEATQUANT = 1.0E-6M;

        public Measure measure;
        public decimal beatpos;                     //beat's loc in measure in duration (ie on the 2nd beat of the measure)
        public List<Symbol> symbols;

        //public static int a = 6;
        //public static int b = 6;
        //public static int c = 14;
        //public int tick;
        //public int sympos;

        public float measpos;                       //beat's pos in measure in pixels
        //public float top;
        public float xpos;
        public float width;
        //public bool hasSharp;

        public int ledgerLinesAbove;
        public bool ledgerLinesMiddle;
        public int ledgerLinesBelow;        

        public Beat(Measure _measure, decimal _beatpos)
        {
            measure = _measure;
            beatpos = _beatpos;
            width = 20;                     //hardwired for now
            xpos = 0;

            symbols = new List<Symbol>();

            //tick = (beat * measure.staff.division) / Measure.quantization;
            //width = a + c;
            //hasSharp = false;

            ledgerLinesAbove = 0;
            ledgerLinesMiddle = false;
            ledgerLinesBelow = 0;
        }

        public void dump()
        {
            Console.WriteLine("beat pos: " + beatpos);
            for (int i = 0; i < symbols.Count; i++)
            {
                symbols[i].dump();
            }
        }

        public void addSymbol(Symbol sym)
        {
            if (sym != null)
            {
                symbols.Add(sym);
                sym.setBeat(this);
            }

            //if (sym is Note)
            //{
            //    Note note = (Note)sym;
            //    hasSharp |= note.hasSharp;
            //}
        }

        //layout symbols pos inside beat box - determines beat's width + if staff needs to add leger lines for this beat 
        public void layoutSymbols()
        {
            ledgerLinesAbove = 0;
            foreach (Symbol sym in symbols)
            {
                sym.layout();
            }
        }

        //sets beat actual pos on score sheet
        public void setPos(float _pos)
        {
            xpos = measpos + _pos;
            foreach (Symbol sym in symbols)
            {
                sym.setPos(xpos, measure.staff.top);
            }            
        }

        public void paint(Graphics g)
        {
            //int left = xorg + xpos;
            //g.DrawLine(Pens.Blue, xorg, top, xorg, top + Staff.grandHeight);
            //g.DrawLine(Pens.Green, xorg+a, top, xorg+a, top + Staff.grandHeight);

            if (ledgerLinesAbove > 0)
            {
                float linepos = measure.staff.top - measure.staff.spacing;
                for (int i = 0; i < ledgerLinesAbove; i++)
                {
                    g.DrawLine(Pens.Red, xpos - 6, linepos, xpos + 6, linepos);
                    linepos -= measure.staff.spacing;
                }
            }

            if (ledgerLinesMiddle)
            {
                g.DrawLine(Pens.Red, xpos - 6, measure.staff.top + (measure.staff.spacing * 5), 
                    xpos + 6, measure.staff.top + (measure.staff.spacing * 5));
            }

            if (ledgerLinesBelow > 0)
            {
                float linepos = measure.staff.bottom + measure.staff.spacing;
                for (int i = 0; i < ledgerLinesBelow; i++)
                {
                    g.DrawLine(Pens.Red, xpos - 6, linepos, xpos + 6, linepos);
                    linepos += measure.staff.spacing;
                }
            }


            foreach (Symbol sym in symbols)
            {
                sym.paint(g);
            }
        }
    }
}
