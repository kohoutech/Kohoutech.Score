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
using System.Drawing;
using System.Drawing.Drawing2D;

using Transonic.Score.Symbols;

namespace Transonic.Score
{
    public class Staff
    {
        public static int minWidth = 48;
        
        public ScoreDoc score;
        public Part part;

        public List<Measure> measures;

        public float spacing;
        public float top;
        public float left;
        public float bottom;
        public float width;
        public float separation;

        public Staff(Part _part, float _spacing)
        {
            part = _part;
            score = part.score;

            measures = new List<Measure>();

            spacing = _spacing;
            top = 0;
            left = 0;
            width = 0;
            separation = spacing * 4;
            bottom = top + (spacing * 8) + separation;
        }

        public void dump()
        {
            for (int i = 0; i < measures.Count; i++)
            {
                measures[i].dump();
            }
        }

        public void layoutMeasures()
        {
            float staffpos = 0;
            foreach (Measure measure in measures)
            {
                measure.staffpos = staffpos;
                measure.layoutBeats();
                staffpos += measure.width;
            }
            width = staffpos;
            if (width < minWidth) width = minWidth;
        }

        public void setPos(float xpos, float ypos)
        {
            left = xpos;
            top = ypos;
            bottom = top + (spacing * 8) + separation;

            foreach (Measure measure in measures)
            {
                measure.setPos(left);
            }
        }

        //public void setSize(float _width, float _height)
        //{
        //    separation = _height;
        //    width = _width;
        //    bottom = top + (spacing * 8) + separation;
        //}

//- display -------------------------------------------------------------------

        public void drawStaff(Graphics g, float ypos)
        {
            float right = left + width;
            for (int i = 0; i < 5; i++)
            {
                g.DrawLine(Pens.Black, left, ypos, right, ypos);
                ypos += spacing;
            }
        }

        public void paint(Graphics g)
        {
            //staves
            float ypos = top;
            drawStaff(g, ypos);                                 //treble clef
            ypos = top + (4 * spacing) + separation;
            drawStaff(g, ypos);                                 //bass clef

            //left barline
            g.DrawLine(Pens.Black, left, top, left, bottom);

            //measures
            float xpos = 0;
            for (int i = 0; i < measures.Count; i++)
            {
                Measure measure = measures[i];
                measure.paint(g);
                xpos += measure.width;
            }

            //current pos marker
            float linepos = score.curStaffPos;
            g.DrawLine(Pens.Green, linepos, g.ClipBounds.Top, linepos, g.ClipBounds.Bottom);
        }
    }
}

//Console.WriteLine("there's no sun in the shadow of the wizard");
