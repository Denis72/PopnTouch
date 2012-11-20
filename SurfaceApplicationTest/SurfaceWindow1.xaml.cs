using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Surface;
using Microsoft.Surface.Presentation;
using Microsoft.Surface.Presentation.Controls;
using Microsoft.Surface.Presentation.Input;
using Microsoft.Xna.Framework.Audio;
using System.Timers;

namespace SurfaceApplicationTest
{
    /// <summary>
    /// Interaction logic for SurfaceWindow1.xaml
    /// </summary>
    public partial class SurfaceWindow1 : SurfaceWindow
    {
        //Définition du tableau global d'offset pour applatissement de portée
        int[] offsettab = new int[] {0,0,0,14,25,37,47,56,64,71,76,80,83,85,85,84,80,75,68,60,50,38,26,15,4,-3,-9,-11,-12,-11,-7};

        Dictionary<int, String> dictionary = new Dictionary<int, string>();
        static int bpm = 90;

        private ScatterViewItem svitem;

        private Music bs = new Music();
        private Stave stave1 = new Stave(2, 30);
        private Stave stave2 = new Stave(3, 30);

        private Tree t;
        private Tree t2;

        private void initDico()
        {
            dictionary.Add(605, "E6");
            dictionary.Add(555, "G6");
            dictionary.Add(505, "B6");
            dictionary.Add(455, "D7");
            dictionary.Add(405, "F7");
            dictionary.Add(335, "E7");
            dictionary.Add(285, "G7");
            dictionary.Add(235, "B7");
            dictionary.Add(185, "D6");
            dictionary.Add(135, "F6");
        }

        private List<Bubble> bubbles { get; set; }
        public static int nbBubble; //incrémente à chaque creation de bulle, pour lui donner un identifiant

        /// <summary>
        /// Charge le contenu audio de la banque son
        /// </summary>
        private void LoadAudioContent()
        {
            stave1.initNoteArray();
            stave2.initNoteArray();

            initDico();
            initiateTree();
        }
        
        /// <summary>
        /// Default constructor.
        /// </summary>
        public SurfaceWindow1()
        {
            InitializeComponent();

            // Add handlers for window availability events
            AddWindowAvailabilityHandlers();

            LoadAudioContent();

  
        }

        /// <summary>
        /// Occurs when the window is about to close. 
        /// </summary>
        /// <param name="e"></param>
        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);

            // Remove handlers for window availability events
            RemoveWindowAvailabilityHandlers();
        }

        /// <summary>
        /// Adds handlers for window availability events.
        /// </summary>
        private void AddWindowAvailabilityHandlers()
        {
            // Subscribe to surface window availability events
            ApplicationServices.WindowInteractive += OnWindowInteractive;
            ApplicationServices.WindowNoninteractive += OnWindowNoninteractive;
            ApplicationServices.WindowUnavailable += OnWindowUnavailable;
        }

        /// <summary>
        /// Removes handlers for window availability events.
        /// </summary>
        private void RemoveWindowAvailabilityHandlers()
        {
            // Unsubscribe from surface window availability events
            ApplicationServices.WindowInteractive -= OnWindowInteractive;
            ApplicationServices.WindowNoninteractive -= OnWindowNoninteractive;
            ApplicationServices.WindowUnavailable -= OnWindowUnavailable;
        }

        /// <summary>
        /// This is called when the user can interact with the application's window.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnWindowInteractive(object sender, EventArgs e)
        {
            //TODO: enable audio, animations here
        }

        /// <summary>
        /// This is called when the user can see but not interact with the application's window.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnWindowNoninteractive(object sender, EventArgs e)
        {
            //TODO: Disable audio here if it is enabled

            //TODO: optionally enable animations here
        }

        /// <summary>
        /// This is called when the application's window is not visible or interactive.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnWindowUnavailable(object sender, EventArgs e)
        {
            //TODO: disable audio, animations here
        }


        /************Méthodes utilisées lors des événements***********/

        private void play_Click(object sender, RoutedEventArgs e)
        {
            stop.Visibility = Visibility.Visible;
            play.Visibility = Visibility.Hidden;
            stave1.initPlay(bpm);
            stave2.initPlay(bpm);
        }

        private void stop_Click(object sender, RoutedEventArgs e)
        {
            stop.Visibility = Visibility.Hidden;
            play.Visibility = Visibility.Visible;
            stave1.initStop();
            stave2.initStop();
        }
/*
        private void RemoveAllEffects(ScatterView sv)
        {
            ScatterViewItem Bubble;
            Microsoft.Surface.Presentation.Generic.SurfaceShadowChrome ssc;
            int max = sv.Items.Count;
            for (int i = 0; i < max; i++)
            {
                Bubble = sv.Items.GetItemAt(i) as ScatterViewItem;
                Bubble.Background = Brushes.Transparent;
                Bubble.ShowsActivationEffects = false;
         
                ssc = Bubble.Template.FindName("shadow", Bubble) as Microsoft.Surface.Presentation.Generic.SurfaceShadowChrome;
                ssc.Visibility = Visibility.Hidden;
            }
        }
    */
        private void Change_Res(object sender, RoutedEventArgs e)
        {
            ResoBox.Visibility = Visibility.Visible;
            GetWidth.Focus();
        }

        //Bouton boite de résolution
        private void SurfaceButton_Click(object sender, RoutedEventArgs e)
        {
            ResoBox.Visibility = Visibility.Collapsed;
        }

        private void generateBubble(object sender, TouchEventArgs e)
        {
            if (Bubble.ScatterViewIsNotSetted())
            {
                Bubble.setScatterView(bubble_zone);
                bubbles = new List<Bubble>();
            }

            //à mettre dans la partie viewBubble
            svitem = new ScatterViewItem();
            svitem.CanScale = false;
            svitem.HorizontalAlignment = HorizontalAlignment.Center;
            svitem.ContainerManipulationCompleted += TouchLeaveBubble;
            svitem.CanScale = false;
            svitem.CanRotate = false;
            svitem.HorizontalAlignment = HorizontalAlignment.Center;

            bubbles.Add(new Bubble(nextBulle(), svitem));

        }

        private void anim(object sender, TouchEventArgs e)
        {
            bubbles.Last().beginAnimation();
        }

        private int nextBulle()
        {
            int[] type = { 0, 0, 0 };

            if (bubbles != null)
            {
                foreach (Bubble bubble in bubbles)
                {
                    type[bubble.id]++;
                }
            }
            List<int> mins = minimum_array(type);

            Random r = new Random();
            return mins[r.Next(mins.Count)];
        }

        private List<int> minimum_array(int[] types)
        {
            List<int> res = new List<int>(3);
            int min = types.Min();
            int i = 0;

            foreach (int a in types)
            {
                if (a == min)
                    res.Add(i);
                i++;
            }

            return res;
        }

        private void endAnimation(object sender, EventArgs e)
        {
            MessageBox.Show("oui");
        }

        private void TouchLeaveBubble(object sender, ContainerManipulationCompletedEventArgs e)
        {
            int durationNote = 0;
            ScatterViewItem bubble = new ScatterViewItem();
            bubble = e.Source as ScatterViewItem;
            Point bubbleCenter = bubble.ActualCenter;

            foreach (Bubble i in bubbles)
            {
                if (bubble.Name == ("b" + i.name))
                    durationNote = i.duration;
            }

            int width = int.Parse(GetWidth.Text);
            int height = int.Parse(GetHeight.Text);
            //int width = 1920;
            //int height = 1080;
            bubbleCenter.X = bubbleCenter.X * 1920 / width;
            bubbleCenter.Y = bubbleCenter.Y * 1080 / height;



            if (bubbleCenter.X <= 90) bubbleCenter.X = 120;
            else if (bubbleCenter.X >= 1830) bubbleCenter.X = 1800;
            else bubbleCenter.X = Math.Floor((bubbleCenter.X + 30) / 60) * 60;

            //"Applatissement" de la portée (MAJ : Switch -> Tableau !)
            int offset=offsettab[((long)bubbleCenter.X/60)];
            bubbleCenter.Y += offset;

          
            //Y dans le cadre portée ?
            //Si oui, animation
            //pas de else
            if (bubbleCenter.Y < 630 && bubbleCenter.Y > 105)
            {
                if (bubbleCenter.Y < 370)
                {
                    if (bubbleCenter.Y >= 335) bubbleCenter.Y = 335;
                    bubbleCenter.Y = Math.Floor((bubbleCenter.Y - 10) / 50) * 50 + 35;

                }
                else
                {
                    if (bubbleCenter.Y <= 405) bubbleCenter.Y = 405;
                    bubbleCenter.Y = Math.Floor((bubbleCenter.Y + 20) / 50) * 50 + 5;
                }

                //Partie son*******************************
                //Le son se déclenche quand la bulle est lachée
                //En fonction de sa portée, on place la note dans le tableau qui convient

                String res = "silence";
                int centreY = (int)bubbleCenter.Y;
                res = dictionary[centreY];

                int centreX = (int)bubbleCenter.X;
                {
                    if (centreY >= 335)
                        stave1.fillArray(centreX, centreY, durationNote, res);

                    else
                    {
                       stave2.fillArray(centreX, centreY, durationNote, res);
                    }   
                }

                //fin Partie son*****************************


                bubbleCenter.Y -= offset;

                bubbleCenter.X = bubbleCenter.X * width / 1920;
                bubbleCenter.Y = bubbleCenter.Y * height / 1080;

                Storyboard stb = new Storyboard();
                PointAnimation moveCenter = new PointAnimation();

                moveCenter.From = bubble.ActualCenter;
                moveCenter.To = bubbleCenter;
                moveCenter.Duration = new Duration(TimeSpan.FromSeconds(0.15));
                bubble.Center = bubbleCenter;
                moveCenter.FillBehavior = FillBehavior.Stop;

                stb.Children.Add(moveCenter);

                Storyboard.SetTarget(moveCenter, bubble);
                Storyboard.SetTargetProperty(moveCenter, new PropertyPath(ScatterViewItem.CenterProperty));

                stb.Begin(this);


                bubble.Visibility = Visibility.Collapsed;
                bubble.Visibility = Visibility.Visible;


            }
        }

        private void clearStave(object sender, RoutedEventArgs e)
        {
            stave1.initNoteArray();
            stave2.initNoteArray();
        }

        private void initiateTree()
        {
            t = new Tree("Images/", "piano.png", "ocarina.png", canvasTree1);

            t2 = new Tree("Images/", "ocarina.png", "piano.png", canvasTree2);
        }
    }
}