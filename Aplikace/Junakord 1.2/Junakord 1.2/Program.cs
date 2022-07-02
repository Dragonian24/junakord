using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Junakord_1._2
{
    class Program
    {
        static void Main(string[] args)
        {
            //---------------------
            //  Konstanty
            //---------------------
            string verze = "Junakord 1.3";





            //---------------------
            //proměnné
            //---------------------
            bool exportIncludeChords;
            bool exportAsOneDocument;

            string songBody = "";
            int songNum = 0;
            string songListString = "";
            string songTextBody = "";
            string textContents = "";


            //---------------------
            //  Začátek programu
            //---------------------

            WriteLogo();

            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine("Zmáčkněte libovolnou klávesu pro pokračování");
            Console.ReadKey();
            //needHelp();
            songConversion();

            //---------------------
            //  Ukončení programu
            //---------------------
            Console.Clear();
            Console.WriteLine("Děkuji za použití aplikace " + verze);
            Console.WriteLine("");
            Console.WriteLine("Libovolná klavesa pro ukončení aplikace");
            Console.ReadKey();
            //
            //
            //**************************************************
            




            //---------------------
            //  Metody
            //---------------------



            void WriteLogo() //počáteční vykreslení loga
            {
                string[] logoText = File.ReadAllLines("TextLogo.txt");
                foreach (string line in logoText)
                {
                    Console.WriteLine(line);
                }
                Console.WriteLine(verze);
            }




            void songConversion()
            {
                string tagContainerStart = "<div class=\"container\">";
                string tagContainerEnd = "</div>";

                string directory = Directory.GetCurrentDirectory();
                foreach (string songFile in Directory.EnumerateFiles(directory + "/kekonverzi", "*.txt"))
                {
                    songNum++;
                    string contents = File.ReadAllText(songFile);                    



                    Console.WriteLine();
                    Console.WriteLine();

                    Console.WriteLine("Uneditet contents");
                    Console.WriteLine(contents);

                    Console.WriteLine();
                    Console.WriteLine();

                    //oddělení prvních 4 řádků

                    string songName, songAuthor, songInfo;
                    string[] extractInfo = contents.Split('\n');

                    string tagSongNameStart = "<h1>";
                    string tagSongNameEnd = "</h1>";
                    string tagAuthorStart = "<h3>";
                    string tagAuthorEnd = "</h3>";
                    string tagInfoMainStart = "<div class=\"keychart\" > ";
                    string tagInfoMainEnd = "</div>";
                    string tagInfoPartialStart = "<p class=\"key\" > ";
                    string tagInfoPartialEnd = "</p>";
                    string tagPisnickaStart = "<div class=\"pisnicka\" > ";
                    string tagPisnickaEnd = "</div>";

                    for (int i = 0; i < extractInfo.Length; i++)
                    {
                        if (i < 3)
                        {
                            extractInfo[i] = extractInfo[i].TrimEnd();
                            extractInfo[i] = extractInfo[i].Replace("(", "");
                            extractInfo[i] = extractInfo[i].Replace(")", "");
                        }

                    }

                    songName = extractInfo[0];
                    songAuthor = extractInfo[1];
                    songInfo = extractInfo[2];

                    songListString += songNum + ".    " + songName + " - " + songAuthor + '\n';

                    string[] songSplitInfo = songInfo.Split(',');

                    string innerInfo = tagInfoPartialStart + "Capo: "  +  songSplitInfo[0] + tagInfoPartialEnd + '\n' + tagInfoPartialStart + "Key:" + songSplitInfo[1] + tagInfoPartialEnd;
                    string finishedInfo = tagInfoMainStart + innerInfo + tagInfoMainEnd;
                    

                    string songHeader = "";
                    songHeader += tagSongNameStart + songNum + " " + songName + tagSongNameEnd + '\n' + tagAuthorStart + songAuthor + tagAuthorEnd + '\n' + finishedInfo;
                    
                    Console.WriteLine("");
                    Console.WriteLine("");
                    Console.WriteLine(" Song header");
                    Console.WriteLine(songHeader);
                    Console.WriteLine("");
                    Console.WriteLine("");

                    contents = "";

                    for (int i = 3; i < extractInfo.Length; i++)
                    {
                        Console.WriteLine("Content line: " + i + "---" + extractInfo[i]);
                        contents += extractInfo[i];
                        contents += '\n';

                    }

                    


                    contents = songEditation(contents);

                    contents = tagPisnickaStart + contents + tagPisnickaEnd;
                    contents = songHeader + contents;
                    contents = tagContainerStart + contents + tagContainerEnd;

                    



                    Console.WriteLine("Contets after edit");
                    Console.WriteLine(contents);

                    songBody += contents + '\n'+ '\n';
                    songTextBody += textContents + '\n' + '\n';


                }




                songExport();
                Console.ReadKey();
            }

            string songEditation(string songText)
            {
                string tagHeaderStart = "<h2>";
                string tagHeaderEnd = "</h2>";
                string tagChordStart = "<span class=\"akord\" >";
                string tagChordEnd = "</span>";
                string tagTextChordStart = "<p class=\"akordy\">";
                string tagTextNoChordStart = "<p class=\"text\">";
                string tagTextEnd = "</p>";
                string tagLineBreak = "<br />";

                string newText = "";
                string simpleText = "";

                Console.WriteLine("song text:");
                Console.WriteLine(songText);

                string[] songParagraphs = songText.Split('(');
                string[] textParagraphs = new string[songParagraphs.Length+1];

                int noP = songParagraphs.Length;
                Console.WriteLine("NUMBER of paragraphs: ");
                Console.WriteLine(noP);



                //!!! spliting to paragraphs !!!
                //vvvvvvvvvvvvvvvvvvvvvvvvv

                for (int i = 0; i < songParagraphs.Length; i++)
                {

                    string textParagraph = songParagraphs[i];
                    if (textParagraph.Length < 3) Console.WriteLine("Empty"); //--if paragraph is empty
                    else //-------------------------------------------------------if paragraph is not empty
                    {
                        string[] songLines = textParagraph.Split('\n');  // create array of lines
                        bool[] songLineHasChords = new bool[songLines.Length]; // create array of bools if line contains chords or not




                        //!!! spliting to lines !!!
                        //vvvvvvvvvvvvvvvvvvvvvvvvv

                        for (int j = 0; j < songLines.Length; j++)
                        {                           
                            songLines[j] = songLines[j].TrimEnd();
                            songLineHasChords[j] = songLines[j].Contains("[");  // determines if line has chords
                        }
                        string[] textLines = new string[songLines.Length];

                        // editing lines
                        if (songLines.Length > 0)
                        {

                            songLines[0] = tagHeaderStart + songLines[0];
                            songLines[0] = songLines[0].Replace(")", tagHeaderEnd);
                            Console.WriteLine("Header: " + songLines[0]);

                           
                            // siple text lines
                            for (int l = 0; l < songLines.Length; l++)
                            {
                                if (l == 1)
                                {
                                    textLines[l] = tagTextNoChordStart + songLines[l];
                                }
                                else if (l == songLines.Length - 1)
                                {
                                    textLines[l] = songLines[l] + tagTextEnd;
                                }
                                else if (l == 0)
                                {
                                    textLines[l] = songLines[l];
                                }
                                else
                                {
                                    textLines[l] = songLines[l] + tagLineBreak;

                                }
                            }

                           


                            if (songLines.Length > 1)
                            {
                                if(songLines.Length > 2) // multiple lines
                                {
                                    for (int c = 1; c < songLines.Length; c++)
                                    {
                                        if (c == 1)
                                        {
                                            if (songLineHasChords[c] == true)
                                            {
                                                songLines[c] = tagTextChordStart + songLines[c];
                                            }
                                            else
                                            {
                                                songLines[c] = tagTextNoChordStart + songLines[c];
                                            }
                                        }
                                        else
                                        {
                                            if (songLineHasChords[c] == songLineHasChords[c - 1])
                                            {

                                            }
                                            else
                                            {
                                                if (songLineHasChords[c] == true)
                                                {
                                                    songLines[c] = tagTextChordStart + songLines[c];
                                                }
                                                else
                                                {
                                                    songLines[c] = tagTextNoChordStart + songLines[c];
                                                }
                                            }
                                        }


                                        if (c == songLines.Length - 1)
                                        {

                                            songLines[c] += tagTextEnd;
                                        }
                                        else
                                        {
                                            if (songLineHasChords[c] == songLineHasChords[c + 1])
                                            {
                                                songLines[c] += tagLineBreak;
                                            }
                                            else
                                            {
                                                songLines[c] += tagTextEnd;
                                            }
                                        }
                                    }
                                }
                                else // header + 1 line
                                {
                                    if (songLineHasChords[1] == true)
                                    {
                                        songLines[1] = tagTextChordStart + songLines[1] + tagTextEnd;
                                    }
                                    else
                                    {
                                        songLines[1] = tagTextNoChordStart + songLines[1] + tagTextEnd;
                                    }
                                }
                            }






                            //exeptions
                            else //just header
                            {
                                Console.WriteLine();
                                Console.WriteLine("!!!");
                                Console.WriteLine("Just Header");
                                Console.WriteLine("!!!");
                                Console.WriteLine();
                            }                           
                        }
                        else // no lines
                        {
                            Console.WriteLine();
                            Console.WriteLine("!!!");
                            Console.WriteLine("No lines");
                            Console.WriteLine("!!!");
                            Console.WriteLine();
                        }


                        //!!! merging lines !!!
                        //vvvvvvvvvvvvvvvvvvvvvvvvv

                        songParagraphs[i] = string.Join("\n", songLines);
                        textParagraphs[i] = string.Join("\n", textLines);
                    }
                }




                //!!! merging paragraphs !!!
                //vvvvvvvvvvvvvvvvvvvvvvvvv

                newText = string.Join("\n", songParagraphs);
                simpleText = string.Join("\n", textParagraphs);

                newText = newText.Replace("[", tagChordStart);
                newText = newText.Replace("]", tagChordEnd);

                textContents = simpleText;
                return newText;


            }

            void songExport()
            {
                string exportSimpleText = songTextBody;
                string exportList = songListString;
                string exportedText = songBody;
                string directory = Directory.GetCurrentDirectory();


                string fileName = directory + "/konvertovano" + "/zpevnik.txt";
                string fileNameNoChords = directory + "/konvertovano" + "/zpevnikText.txt";
                string fileNameSongList = directory + "/konvertovano" + "/seznam.txt";


                Console.WriteLine();
                Console.WriteLine();
                Console.WriteLine();
                Console.WriteLine();
                Console.WriteLine("SONG BODY------------------------------------");
                Console.WriteLine();
                Console.WriteLine(songBody);

                if (File.Exists(fileName))
                {
                    File.Delete(fileName);
                }
                if (File.Exists(fileNameNoChords))
                {
                    File.Delete(fileNameNoChords);
                }
                if (File.Exists(fileNameSongList))
                {
                    File.Delete(fileNameSongList);
                }

                File.WriteAllText(fileNameSongList, exportList);
                File.WriteAllText(fileNameNoChords, exportSimpleText);
                File.WriteAllText(fileName, exportedText);
            }

            //ukončení
            Console.WriteLine("Konverze hotova");
            Console.WriteLine("Soubory naleznete ve složce: /Konvertovano");
            Console.WriteLine("Pokračujte stisknutim libovolné klávesy");
            Console.ReadKey();
            //EndAplication();
            
        }      
    }
}
