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
    public class Symbol
    {
        public Measure measure;
        public int startTick;           //start tick relative to measure start
        public int duration;            //length in ticks

        public int beat;                //beat that symbol starts on
        public int len;                 //symbol len in beats

        public int xpos;                //symbol pos from measure left in pixels
        public int ypos;                //symbol pos from measure top in pixels

        public Symbol()
        {
            measure = null;
            startTick = 0;
            duration = 0;
            beat = 0;
            len = 0;
            xpos = 0;
            ypos = 0;
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

    public class BarLine : Symbol
    {
        public BarLine()
        {
        }
    }
    
//-----------------------------------------------------------------------------

    public class Rest : Symbol
    {
        public Rest(int _start, int _len) : base()
        {
            beat = _start;
            len = _len;
        }

        public override void paint(Graphics g, int left, int top)
        {
            g.FillRectangle(Brushes.Blue, left + 6, top + Staff.lineSpacing, 8, 4);

        }
    }

}
