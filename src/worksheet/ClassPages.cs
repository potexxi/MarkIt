using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Shapes;

namespace MarkIt.worksheet
{
    public class ClassPages
    {
        private double height { get; set; }
        private double width { get; set; }
        public string content { get; set; }
        private Grid page;

        public ClassPages(Grid page) { this.page = page;}
        public ClassPages(double Height, double Width, string Content, Grid page):this(page)
        {
            this.height = Height;
            this.width = Width;
            this.content = Content;
        }
        
        public Grid Render()
        // renders the current page and returns the rendert page
        {
            locateFormat();
            return this.page;
        }


        public List<List<int>> locateFormat()
        // sees if the Program can find formatations
        {
            List<List<int>> found = new(); // shortent version (got that from the towerdefense excercise)

            List<List<int>> foundheader = findSymbole(content, '#');
            for (int i = 0; i < foundheader.Count(); i++)
                if (foundheader[i][0] == 0)
                {
                    if (foundheader[i][2] == 1) // header 1
                        found.Add([foundheader[i][0], foundheader[i][1], foundheader[i][2]]);
                    else if (foundheader[i][2] == 2) // header 2
                        found.Add([foundheader[i][0], foundheader[i][1], foundheader[i][2]]);
                    else if (foundheader[i][2] == 3) // header 3
                        found.Add([foundheader[i][0], foundheader[i][1], foundheader[i][2]]);
                }
            List<List<int>> foundformat = findSymbole(content, '*');
            for (int i = 0; i < foundformat.Count(); i++)
            {
                if (foundformat[i][2] == 1) // italic
                    found.Add([foundformat[i][0], foundformat[i][1], foundformat[i][2]]);
                else if (foundformat[i][2] == 2) // bold
                    found.Add([foundformat[i][0], foundformat[i][1], foundformat[i][2]]);
                else if (foundformat[i][2] == 3) // bold italic
                    found.Add([foundformat[i][0], foundformat[i][1], foundformat[i][2]]);
            }
            return found;
        }
        public int AmountSymbelChainsPerLine(List<List<int>> found)
        {
            int[] lines = [];
            foreach(List<int> line in found)
            {
                lines[line[2]] = 2;
            }
            return 1;
        }

        public List<List<int>> findSymbole(string txt, char symbol)
        // finds a symbole inside of a string
        {
            List<List<int>> foundchains = new(); // shortent version (got that from the towerdefense excercise)
            int letterNumber = 0;
            int lineNumber = 0;
            int chainlength = 0; // shows the cohearend <- (vallahi not chatgpt :^D ) number of symbels in a row;
            if(txt != null)
            {
                string[] splited = txt.Split("\n");
                foreach (string line in splited)
                {
                    lineNumber++;
                    letterNumber = 0;
                    for (int i = 0; i < line.Count(); i++, letterNumber++)
                    {
                        if (line[i] == symbol)
                        {
                            chainlength = 0;
                            int remaining = line.Count() - letterNumber;
                            for (int chainstart = letterNumber; chainstart < line.Length && line[chainstart] == symbol; chainstart++)
                                chainlength++;
                            List<int> entry = [letterNumber, lineNumber, chainlength];
                            foundchains.Add(entry);
                            letterNumber += chainlength - 1; // that it doesn't go over existing lines
                        }
                    }
                }
            }

            return foundchains;
        }
    }
}
