using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using NAudio;
using NAudio.Wave;


namespace Grabacion
{
    /// <summary>
    /// Lógica de interacción para MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        WaveIn wavein;
        WaveFormat formato;
        WaveFileWriter writer;
        AudioFileReader reader;
        WaveOutEvent waveOut;


        public MainWindow()
        {
            InitializeComponent();
        }

        private void btniniciar_Click(object sender, RoutedEventArgs e)
        {
            
            wavein = new WaveIn();
            wavein.WaveFormat = new WaveFormat(44100, 16, 1);
            formato = wavein.WaveFormat;

            wavein.DataAvailable += OnDaraAvailable;
            wavein.RecordingStopped += OnRectodingStopped;
            writer =
                new WaveFileWriter("sonido.wav", formato);

            wavein.StartRecording();
        }

        void OnRectodingStopped(object sender, StoppedEventArgs e)
        {
            writer.Dispose();
        }

        void OnDaraAvailable(object sender, WaveInEventArgs e)
        {
            byte[] buffer = e.Buffer;
            int bytesGrabados = e.BytesRecorded;

            for (int i=0; i < bytesGrabados; i++)
            {
                lblmuestras.Content= buffer[i].ToString();
            }

            writer.Write(buffer, 0, bytesGrabados);
        }

        private void btnfinalizar_Click(object sender, RoutedEventArgs e)
        {
            wavein.StopRecording();
        }

        private void btnReproducir_Click(object sender, RoutedEventArgs e)
        {
            reader = new AudioFileReader("sonido.wav");
            waveOut = new WaveOutEvent();
            waveOut.Init(reader);
            waveOut.Play();
        }
    }
}
