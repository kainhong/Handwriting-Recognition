using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Microsoft.Ink;

namespace 手写识别
{
    public partial class Form1 : Form
    {
        InkCollector ic;
        RecognizerContext rct;
       // Recognizer rc;
        string FullCACText;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //this.rct.RecognitionWithAlternates += new RecognizerContextRecognitionWithAlternatesEventHandler(rct_RecognitionWithAlternates);

            ic = new InkCollector(ink_here.Handle);
            this.ic.Stroke += new InkCollectorStrokeEventHandler(ic_Stroke);

            ic.Enabled = true;
            ink_();

         //   this.ic.Stroke += new InkCollectorStrokeEventHandler(ic_Stroke);
          this.rct.RecognitionWithAlternates += new RecognizerContextRecognitionWithAlternatesEventHandler(rct_RecognitionWithAlternates);

           rct.Strokes = ic.Ink.Strokes;
           
          

        }

        void rct_RecognitionWithAlternates(object sender, RecognizerContextRecognitionWithAlternatesEventArgs e)
        {
           
             string ResultString="";
             RecognitionAlternates alts;
         
            if (e.RecognitionStatus == RecognitionStatus.NoError)            
            {
                alts = e.Result.GetAlternatesFromSelection();
                
              foreach(RecognitionAlternate alt in alts)
              {
                  ResultString = ResultString + alt.ToString()+" ";                  
              }        
            }
            FullCACText = ResultString.Trim();
            Control.CheckForIllegalCrossThreadCalls=false;            
            textBox1.Text = FullCACText;
            返回识别字符处理(FullCACText);
            Control.CheckForIllegalCrossThreadCalls =true;            

        }

        private void 返回识别字符处理(string 字符)
        {
            string[] str_temp = 字符.Split(' ');
            string str_temp1 = "shibie_";
            string str_temp2 = "";
            if (str_temp.Length > 0)
            {
                for (int i = 0; i < str_temp.Length; i++)
                {
                    str_temp2 = str_temp1 + i.ToString();
                    Control[] con_temp = Controls.Find(str_temp2, true);
                    if (con_temp.Length > 0)
                    {
                        ((Button)(con_temp[0])).Text = str_temp[i];
                    }
                }
            }
        }

        void ic_Stroke(object sender, InkCollectorStrokeEventArgs e)
        {
            rct.StopBackgroundRecognition();
            rct.Strokes.Add(e.Stroke);
            rct.CharacterAutoCompletion = RecognizerCharacterAutoCompletionMode.Full;
            rct.BackgroundRecognizeWithAlternates(0);
        }

        private void ink_()
        {
            Recognizers recos = new Recognizers();           
            Recognizer chineseReco  = recos.GetDefaultRecognizer();

            rct = chineseReco.CreateRecognizerContext();
        }
        private void ic_Stroke()
        {
 
        }

        private void button3_Click(object sender, EventArgs e)
        {
            colorDialog1.FullOpen = true;
            colorDialog1.ShowDialog();
            ic.DefaultDrawingAttributes.Color = colorDialog1.Color;

        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (!ic.CollectingInk)
            {
                Strokes strokesToDelete = ic.Ink.Strokes;                
                rct.StopBackgroundRecognition();
                ic.Ink.DeleteStrokes(strokesToDelete);
                rct.Strokes = ic.Ink.Strokes;
                ic.Ink.DeleteStrokes();//清除手写区域笔画;
                ink_here.Refresh();//刷新手写区域
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
           
          
            
            textBox1.SelectedText = ic.Ink.Strokes.ToString();
          /*  if (0 == rc.Count)
            {
                MessageBox.Show("There are no handwriting recognizers installed.  You need to have at least one in order to recognize ink.");
            }
            else
            {

                // Note that the Strokes' ToString() method is a 
                // shortcut  for retrieving the best match using the  
                // default recognizer.  The same result can also be 
                // obtained using the RecognizerContext.  For an 
                // example, uncomment the following lines of code:
                // 
                 RecognizerContext myRecoContext = new RecognizerContext();
                 myRecoContext.CharacterAutoCompletion = RecognizerCharacterAutoCompletionMode.Full;
                 RecognitionStatus status;
                RecognitionResult recoResult;
                //
                 myRecoContext.Strokes = ic.Ink.Strokes;
                recoResult = myRecoContext.Recognize(out status);
                textBox1.SelectedText = recoResult.TopString;
                //
                
               // textBox1.SelectedText = ic.Ink.Strokes.ToString();
            }*/
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
    }
}