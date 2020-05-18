/* ----------------------------------------------------------------------------
Kohoutech Score Library
Copyright (C) 1997-2020 George E Greaney

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

using Kohoutech.Score.Symbols;

namespace Kohoutech.Score
{
    public class Measure
    {
        //dimensions based upon typograhy in Beethoven's Complete Piano Sonatas, Dover ed.
        public static int minWidth = 48;
        public const int quantization = 8;         //quantize rests to 1/32 note pos (quarter note / 8)

        public Staff staff;
        public Measure prevMeasure;
        public Measure nextMeasure;

        public Attributes attr;             //the time & key sigs for this measure, may have been defined in a prev measure
        public TimeSignature timeSig;       //if this is set, we draw a time sig at the draw of this measure
        public KeySignature keySig;         //same with key sig
        public int[] accidentals;

        public List<Beat> beats;        

        public int number;                  //measure number 
        public decimal length;              //number of beats in measure in divisions, determined by most recent key signature

        public float staffpos;                //ofs in staff in measure, in pixels
        public float width;                   //width of measure, sum of all beat widths, in pixels
        public float xpos;

        public int curBeatNum;
        public Beat curBeat;

        public Measure(int _number, Measure prev)
        {
            staff = null;
            number = _number;
            prevMeasure = prev;
            if (prevMeasure != null)
            {
                prevMeasure.nextMeasure = this;
                attr = prevMeasure.attr;            //use prev measure's attr, unless replaced in this measure
            }
            else
            {
                attr = new Attributes();            //default time/key sig is this first measure, may be replaced below
            }
            nextMeasure = null;

            timeSig = null;
            keySig = null;

            beats = new List<Beat>();
            length = (attr.divisions * 4 * attr.timeNumer) / attr.timeDenom;

            staffpos = 0;
            width = 0;
            curBeatNum = -1;
            curBeat = null;
        }

        public void setAttributes(Attributes attr)
        {
            if (attr.beatpos == 0)          //only attributes at the start of a measure for now
            {
                this.attr = attr;
                if (!attr.haveDivisions)
                {
                    attr.divisions = prevMeasure.attr.divisions;
                }
                timeSig = new TimeSignature(this, attr.timeNumer, attr.timeDenom);
                keySig = new KeySignature(this, attr.key);
                length = (attr.divisions * 4 * attr.timeNumer) / attr.timeDenom;
            }
        }

        public void setPrint(Print print)
        {
        }

        public void setBarLine(Barline barline)
        {
        }

        public void dump()
        {
            Console.WriteLine("measure number " + number);
            for (int i = 0; i < beats.Count; i++)
            {
                beats[i].dump();
            }
        }


//- beats -------------------------------------------------------------------

        public Beat getBeat(decimal beatPos)
        {
            Beat result = null;
            foreach (Beat beat in beats)
            {
                if (Math.Abs(beat.beatpos - beatPos) < Beat.BEATQUANT)
                {
                    result = beat;
                    break;
                }
            }
            if (result == null)
            {
                result = new Beat(this, beatPos);
                beats.Add(result);
                beats.Sort((a, b) => a.beatpos.CompareTo(b.beatpos));        
            }
            return result;
        }

//- layout -------------------------------------------------------------------

        ////insert a rest at any point in the measure there isn't a note playing
        //public void insertRests()
        //{
        //    if (symbols.Count == 0)     //no notes at all, insert measure long rest
        //    {
        //        Rest rest = new Rest(0, timeNumer * quantization);
        //        symbols.Add(rest);
        //        rest.setMeasure(this);
        //    }
        //    else
        //    {
        //        List<Symbol> syms = new List<Symbol>();
        //        int beat = 0;
        //        for (int i = 0; i < symbols.Count; i++)
        //        {
        //            Note note = (Note)symbols[i];
        //            if (note.beat > beat)
        //            {
        //                int restLen = note.beat - beat;
        //                Rest rest = new Rest(beat, restLen);
        //                syms.Add(rest);
        //                rest.setMeasure(this);
        //            }
        //            if (beat < (note.beat + note.len))
        //            {
        //                beat = (note.beat + note.len);
        //            }
        //            syms.Add(note);
        //        }
        //        symbols = syms;
        //        if (beat < (timeNumer * quantization))                  //any remaining time in measure
        //        {
        //            int restLen = (timeNumer * quantization) - beat;
        //            Rest rest = new Rest(beat, restLen);
        //            symbols.Add(rest);
        //            rest.setMeasure(this);
        //        }
        //    }
        //}

        //layout beats inside measure - determine measure's width
        public void layoutBeats()
        {
        //    insertRests();
        //    //BarLine barline = new BarLine();
        //    //barline.start = timeNumer;
        //    //barline.startTick = staff.division * timeNumer;
        //    //symbols.Add(barline);

            //init accidentals
            accidentals = new int[(9 * 7)];
            int accidnum = 0;            
            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 7; j++) {
                accidentals[accidnum++] = Note.circleOfFifths[attr.key + 6, j];
            }
            }

            float beatPos = 0;             //hardwired offset of first beat in measure, will change
            if (keySig != null)
            {
                keySig.layout(beatPos);
                beatPos += keySig.width;
            }
            if (timeSig != null)
            {
                timeSig.layout(beatPos);
                beatPos += timeSig.width;
            }
            beatPos += 20;
            foreach (Beat beat in beats)
            {
                beat.measpos = beatPos;
                beat.layoutSymbols();
                beatPos += beat.width;                
            }

            width = beatPos;
            if (width < minWidth) width = minWidth;
        }

        public void setPos(float _xpos)
        {
            xpos = staffpos + _xpos;
            if (keySig != null)
            {
                keySig.setPos(xpos);                
            }
            if (timeSig != null)
            {
                timeSig.setPos(xpos);
            }
            foreach (Beat beat in beats)
            {
                beat.setPos(xpos);
            }
        }

        public void setCurrentBeat(decimal beatNum)
        {
            int i = 0;
            while ((i < beats.Count - 1) && (beatNum >= (beats[i+1].beatpos / attr.divisions)))
                i++;
            curBeat = beats[i];
        }

        ////translate tick to pixels for this measure
        //public int getBeatPos(int tick)
        //{
        //    tick -= startTick;
        //    int i = 0;
        //    while ((i < beats.Count - 1) && (tick > beats[i+1].tick))
        //        i++;
        //    int pos = beats[i].sympos;
        //    return pos; 
        //}

//- display -------------------------------------------------------------------
 
        public void paint(Graphics g)
        {            

            //measure num
            g.DrawString(number.ToString(), SystemFonts.DefaultFont, Brushes.Black, xpos, staff.top - 14);

            if (keySig != null)
            {
                keySig.paint(g);                
            }
            if (timeSig != null)
            {
                timeSig.paint(g);
            }

            //beats
            for (int i = 0; i < beats.Count; i++)
            {
                beats[i].paint(g);
            }

            //barline
            g.DrawLine(Pens.Black, xpos + width, staff.top, xpos + width, staff.bottom);
        }
    }

//- measure attributes --------------------------------------------------------

    public enum KEYMODE
    {
        Major, 
        Minor, 
        Dorian, 
        Phrygian, 
        Lydian, 
        Mixolydian, 
        Aeolian, 
        Ionian, 
        Locrian, 
        None
    }

    public class Attributes 
    {
        public decimal divisions;
        public bool haveDivisions;
        public int timeNumer;
        public int timeDenom;
        public int key;
        public KEYMODE mode;

        public decimal beatpos;

        public Attributes()
        {
            divisions = 1;
            haveDivisions = false;
            timeNumer = 4;
            timeDenom = 4;
            key = 0;
            mode = KEYMODE.Major;       //def time sig = 4/4, key sig = C major

            beatpos = 0;
        }
    }

//-----------------------------------------------------------------------------

    public class TimeSignature
    {
        public Measure measure;
        public int numer;
        public int denom;

        public float measpos;                       //beat's pos in measure in pixels
        public float xpos;
        public float width;

        public TimeSignature(Measure _meas, int _numer, int _denom)
        {
            measure = _meas;
            numer = _numer;
            denom = _denom;
        }

        public void layout(float _pos)
        {
            measpos = _pos;
            width = 20;
        }

        public void setPos(float _pos)
        {
            xpos = measpos + _pos;
        }
            
        public void paint(Graphics g)
        {
            Staff staff = measure.staff;
            Font timefont = new Font("Arial", 14);

            //treble clef
            g.DrawString(numer.ToString(), timefont, Brushes.Black, xpos, staff.top - 2);
            g.DrawString(denom.ToString(), timefont, Brushes.Black, xpos, staff.top + (staff.spacing * 2) - 2);

            //bass clef
            g.DrawString(numer.ToString(), timefont, Brushes.Black, xpos, staff.top + (staff.spacing * 4) - 2 + staff.separation);
            g.DrawString(denom.ToString(), timefont, Brushes.Black, xpos, staff.top + (staff.spacing * 6) - 2 + staff.separation);

            timefont.Dispose();
        }
    }

//-----------------------------------------------------------------------------

    public class KeySignature
    {
        public Measure measure;
        public int key;

        public float measpos;                       //beat's pos in measure in pixels
        public float xpos;
        public float width;

        public KeySignature(Measure _meas, int _key)
        {
            measure = _meas;
            key = _key;
            xpos = 0;
            width = 0;
        }

        public void layout(float _pos)
        {
            measpos = _pos;
            width = measure.staff.spacing * (key) + 10;
        }

        public void setPos(float _pos)
        {
            xpos = measpos + _pos;
        }

        float[] sharpYPos = { 0.0f, 1.5f, -0.5f, 1.0f, 2.5f, 0.5f };
        float[] flatYPos = { 2.0f, 0.5f, 2.5f, 1.0f, 3.0f, 1.5f };

        public void paint(Graphics g)
        {
            float ypos = measure.staff.top;
            Staff staff = measure.staff;
            Font keyfont = new Font("Arial", 14);
            float y = staff.top - 12;
            for (int j = 0; j < 2; j++)
            {
                if (key > 0)
                {
                    float x = xpos;
                    for (int i = 0; i < key; i++)
                    {
                        g.DrawString("\u266f", keyfont, Brushes.Black, x, y + (sharpYPos[i] * staff.spacing));
                        x += staff.spacing;
                    }
                }
                else
                {
                    int count = -key;
                    float x = xpos;
                    for (int i = 0; i < count; i++)
                    {
                        g.DrawString("\u266d", keyfont, Brushes.Green, x, y + (sharpYPos[i] * staff.spacing));
                        x += staff.spacing;
                    }                    
                }
                y += (staff.spacing * 5) + staff.separation;
            }
            keyfont.Dispose();
        }
    }


}

//Console.WriteLine("there's no sun in the shadow of the wizard");
