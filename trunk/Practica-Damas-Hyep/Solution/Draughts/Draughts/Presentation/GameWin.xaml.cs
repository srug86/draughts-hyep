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
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Draughts.Domain;
using Draughts.Communications;

namespace Draughts.Presentation
{
    /// <summary>
    /// Lógica de interacción para GameWin.xaml.
    /// Ventana de juego para una partida de damas.
    /// </summary>
    public partial class GameWin : Window, Observer
    {
        /// <summary>
        /// Ventana inicial.
        /// </summary>
        private InitWin init;
        /// <summary>
        /// Jugadores de la partida.
        /// </summary>
        private Player pl1, pl2;
        /// <summary>
        /// Instancia de NetMode.
        /// </summary>
        private NetMode net = NetMode.Instance;
        /// <summary>
        /// Objeto de tipo GameActions.
        /// </summary>
        private GameActions gameActions;
        /// <summary>
        /// Diccionario para el contenido de las casillas.
        /// </summary>
        private Dictionary<int, string> colorBox;
        /// <summary>
        /// Diccionario para el mensaje del turno.
        /// </summary>
        private Dictionary<int, string> msgTurn;
        /// <summary>
        /// Tablero de juego.
        /// </summary>
        private Image [,] table;

        /// <summary>
        /// Constructor de la clase <see cref="GameWin"/>.
        /// </summary>
        /// <param name="init">InitWin.</param>
        /// <param name="subject">Observable.</param>
        /// <param name="gameActions">GameActions.</param>
        public GameWin(InitWin init, Subject subject, GameActions gameActions)
        {
            this.gameActions = gameActions;
            this.pl1 = gameActions.Pl1;
            this.pl2 = gameActions.Pl2;
            this.init = init;
            net.Game = this;
            InitializeComponent();
            register(subject);
            textPl1.Text = pl1.Name;
            textPl2.Text = pl2.Name;
            imgPl1.Source = loadImage(this.pl1.Avatar);
            imgPl2.Source = loadImage(this.pl2.Avatar);
            initTable();
        }

        /// <summary>
        /// Cargar una imagen.
        /// </summary>
        /// <param name="path">Ruta.</param>
        /// <returns>Imagen.</returns>
        private BitmapImage loadImage(String path)
        {
            BitmapImage bi = new BitmapImage();
            bi.BeginInit();
            bi.UriSource = new Uri(path, UriKind.Relative);
            bi.EndInit();
            return bi;
        }

        /// <summary>
        /// Inicializar el tablero de juego.
        /// </summary>
        private void initTable()
        {
            this.table = new Image[8, 8] {
            {img1x1, img1x2, img1x3, img1x4, img1x5, img1x6, img1x7, img1x8},
            {img2x1, img2x2, img2x3, img2x4, img2x5, img2x6, img2x7, img2x8},
            {img3x1, img3x2, img3x3, img3x4, img3x5, img3x6, img3x7, img3x8},
            {img4x1, img4x2, img4x3, img4x4, img4x5, img4x6, img4x7, img4x8},
            {img5x1, img5x2, img5x3, img5x4, img5x5, img5x6, img5x7, img5x8},
            {img6x1, img6x2, img6x3, img6x4, img6x5, img6x6, img6x7, img6x8},
            {img7x1, img7x2, img7x3, img7x4, img7x5, img7x6, img7x7, img7x8},
            {img8x1, img8x2, img8x3, img8x4, img8x5, img8x6, img8x7, img8x8}};
        }

        /// <summary>
        /// Espera notifiaciones del observador.
        /// </summary>
        /// <param name="subject">Observable.</param>
        private void register(Subject subject)
        {
            subject.registerInterest(this);
            // Crea el Dictionary
            colorBox = new Dictionary<int, string>();
            colorBox.Add(0, "/img/empty.jpg");       // Casilla vacía
            colorBox.Add(1, "/img/red.jpg");         // Ficha roja
            colorBox.Add(2, "/img/white.jpg");       // Ficha blanca
            colorBox.Add(11, "/img/redQ.jpg");       // Reina roja
            colorBox.Add(22, "/img/whiteQ.jpg");     // Reina blanca
            colorBox.Add(111, "/img/onRed.jpg");     // Ficha roja señalada
            colorBox.Add(222, "/img/onWhite.jpg");   // Ficha blanca señalada
            colorBox.Add(1111, "/img/onRedQ.jpg");   // Reina roja señalada
            colorBox.Add(2222, "/img/onWhiteQ.jpg"); // Reina blanca señalada
            colorBox.Add(11111, "/img/move.jpg");    // Posible movimiento ficha roja
            colorBox.Add(22222, "/img/move.jpg");    // Posible movimiento ficha blanca
            colorBox.Add(111111, "/img/move.jpg");   // Posible movimiento reina roja
            colorBox.Add(222222, "/img/move.jpg");   // Posible movimeinto reina blanca
            msgTurn = new Dictionary<int, string>();
            msgTurn.Add(1, "Juegan rojas");
            msgTurn.Add(2, "Juegan blancas");
            msgTurn.Add(11, "¡¡Ganan rojas!!");
            msgTurn.Add(22, "¡¡Ganan blancas!!");
            msgTurn.Add(33, "¡¡Hay tablas!!");
        }

        /// <summary>
        /// Modifica el contenido de la casilla al recibir una notificación.
        /// </summary>
        /// <param name="row">Fila.</param>
        /// <param name="column">Columna.</param>
        /// <param name="state">Estado.</param>
        public void notify(int row, int column, int state)
        {
            BitmapImage bi = new BitmapImage();
            bi.BeginInit();
            bi.UriSource = new Uri((String)colorBox[state], UriKind.RelativeOrAbsolute);
            bi.EndInit();
            this.table[row,column].Source = bi;
        }

        /// <summary>
        /// Modifica el turno al recibir una notificación.
        /// </summary>
        /// <param name="turn">Turno.</param>
        public void notify(int turn)
        {
            this.lblMessage.Content = (String)msgTurn[turn];
            if (turn > 2)
            {
                LinearGradientBrush labelBrush = new LinearGradientBrush();
                if (turn == 22) labelBrush.GradientStops.Add(new GradientStop(Colors.White, 1));
                else
                {
                    LinearGradientBrush labelBrush2 = new LinearGradientBrush();
                    labelBrush2.GradientStops.Add(new GradientStop(Colors.White, 1));
                    this.lblMessage.Foreground = labelBrush2;;
                    if (turn == 11) labelBrush.GradientStops.Add(new GradientStop(Colors.Red, 1));
                    if (turn == 33) labelBrush.GradientStops.Add(new GradientStop(Colors.Gold, 1));
                }
                this.lblMessage.Background = labelBrush;
            }
        }

        /// <summary>
        /// Manejador para el botón Exit.
        /// </summary>
        /// <param name="sender">Event.</param>
        /// <param name="e">The <see cref="System.Windows.RoutedEventArgs"/> instance containing the event data.</param>
        private void btnExit_Click(object sender, RoutedEventArgs e)
        {
            this.init.Visibility = Visibility.Visible;
            this.Close();
        }

        /// <summary>
        /// Delegado para modificar la ventana de juego.
        /// </summary>
        /// <param name="srcR">Fila origen.</param>
        /// <param name="srcC">Columna origen.</param>
        /// <param name="dstR">Fila destino.</param>
        /// <param name="dstC">Columna destino.</param>
        private delegate void ReceiveCoordinates(int srcR, int srcC, int dstR, int dstC);
        /// <summary>
        /// Método para el delegado.
        /// </summary>
        /// <param name="srcR">Fila origen.</param>
        /// <param name="srcC">Columna origen.</param>
        /// <param name="dstR">Fila destino.</param>
        /// <param name="dstC">Columna destino.</param>
        public void delegateToRcvCoordinates(int srcR, int srcC, int dstR, int dstC)
        {
            ReceiveCoordinates aux = new ReceiveCoordinates(this.clickEnemy);
            Dispatcher.Invoke(System.Windows.Threading.DispatcherPriority.Normal, aux, srcR, srcC, dstR, dstC);
        }

        /// <summary>
        /// Mostrar los cambios en la ventana de juego.
        /// </summary>
        /// <param name="srcR">Fila origen.</param>
        /// <param name="srcC">Columna origen.</param>
        /// <param name="dstR">Fila destino.</param>
        /// <param name="dstC">Columna destino.</param>
        private void clickEnemy(int srcR, int srcC, int dstR, int dstC)
        {
            this.gameActions.selectedBox(srcR, srcC, false);
            this.gameActions.selectedBox(dstR, dstC, false);
        }

        /// <summary>
        /// Manejador para el botón casilla 1x2.
        /// </summary>
        /// <param name="sender">Event.</param>
        /// <param name="e">The <see cref="System.Windows.Input.MouseButtonEventArgs"/> instance containing the event data.</param>
        private void Click_1x2(object sender, MouseButtonEventArgs e)
        {
            this.gameActions.selectedBox(0, 1, true);
        }

        /// <summary>
        /// Manejador para el botón casilla 1x4.
        /// </summary>
        /// <param name="sender">Event.</param>
        /// <param name="e">The <see cref="System.Windows.Input.MouseButtonEventArgs"/> instance containing the event data.</param>
        private void Click_1x4(object sender, MouseButtonEventArgs e)
        {
            this.gameActions.selectedBox(0, 3, true);
        }

        /// <summary>
        /// Manejador para el botón casilla 1x6.
        /// </summary>
        /// <param name="sender">Event.</param>
        /// <param name="e">The <see cref="System.Windows.Input.MouseButtonEventArgs"/> instance containing the event data.</param>
        private void Click_1x6(object sender, MouseButtonEventArgs e)
        {
            this.gameActions.selectedBox(0, 5, true);
        }

        /// <summary>
        /// Manejador para el botón casilla 1x8.
        /// </summary>
        /// <param name="sender">Event.</param>
        /// <param name="e">The <see cref="System.Windows.Input.MouseButtonEventArgs"/> instance containing the event data.</param>
        private void Click_1x8(object sender, MouseButtonEventArgs e)
        {
            this.gameActions.selectedBox(0, 7, true);
        }

        /// <summary>
        /// Manejador para el botón casilla 2x1.
        /// </summary>
        /// <param name="sender">Event.</param>
        /// <param name="e">The <see cref="System.Windows.Input.MouseButtonEventArgs"/> instance containing the event data.</param>
        private void Click_2x1(object sender, MouseButtonEventArgs e)
        {
            this.gameActions.selectedBox(1, 0, true);
        }

        /// <summary>
        /// Manejador para el botón casilla 2x3.
        /// </summary>
        /// <param name="sender">Event.</param>
        /// <param name="e">The <see cref="System.Windows.Input.MouseButtonEventArgs"/> instance containing the event data.</param>
        private void Click_2x3(object sender, MouseButtonEventArgs e)
        {
            this.gameActions.selectedBox(1, 2, true);
        }

        /// <summary>
        /// Manejador para el botón casilla 2x5.
        /// </summary>
        /// <param name="sender">Event.</param>
        /// <param name="e">The <see cref="System.Windows.Input.MouseButtonEventArgs"/> instance containing the event data.</param>
        private void Click_2x5(object sender, MouseButtonEventArgs e)
        {
            this.gameActions.selectedBox(1, 4, true);
        }

        /// <summary>
        /// Manejador para el botón casilla 2x7.
        /// </summary>
        /// <param name="sender">Event.</param>
        /// <param name="e">The <see cref="System.Windows.Input.MouseButtonEventArgs"/> instance containing the event data.</param>
        private void Click_2x7(object sender, MouseButtonEventArgs e)
        {
            this.gameActions.selectedBox(1, 6, true);
        }

        /// <summary>
        /// Manejador para el botón casilla 3x2.
        /// </summary>
        /// <param name="sender">Event.</param>
        /// <param name="e">The <see cref="System.Windows.Input.MouseButtonEventArgs"/> instance containing the event data.</param>
        private void Click_3x2(object sender, MouseButtonEventArgs e)
        {
            this.gameActions.selectedBox(2, 1, true);
        }

        /// <summary>
        /// Manejador para el botón casilla 3x4.
        /// </summary>
        /// <param name="sender">Event.</param>
        /// <param name="e">The <see cref="System.Windows.Input.MouseButtonEventArgs"/> instance containing the event data.</param>
        private void Click_3x4(object sender, MouseButtonEventArgs e)
        {
            this.gameActions.selectedBox(2, 3, true);
        }

        /// <summary>
        /// Manejador para el botón casilla 3x6.
        /// </summary>
        /// <param name="sender">Event.</param>
        /// <param name="e">The <see cref="System.Windows.Input.MouseButtonEventArgs"/> instance containing the event data.</param>
        private void Click_3x6(object sender, MouseButtonEventArgs e)
        {
            this.gameActions.selectedBox(2, 5, true);
        }

        /// <summary>
        /// Manejador para el botón casilla 3x8.
        /// </summary>
        /// <param name="sender">Event.</param>
        /// <param name="e">The <see cref="System.Windows.Input.MouseButtonEventArgs"/> instance containing the event data.</param>
        private void Click_3x8(object sender, MouseButtonEventArgs e)
        {
            this.gameActions.selectedBox(2, 7, true);
        }

        /// <summary>
        /// Manejador para el botón casilla 4x1.
        /// </summary>
        /// <param name="sender">Event.</param>
        /// <param name="e">The <see cref="System.Windows.Input.MouseButtonEventArgs"/> instance containing the event data.</param>
        private void Click_4x1(object sender, MouseButtonEventArgs e)
        {
            this.gameActions.selectedBox(3, 0, true);
        }

        /// <summary>
        /// Manejador para el botón casilla 4x3.
        /// </summary>
        /// <param name="sender">Event.</param>
        /// <param name="e">The <see cref="System.Windows.Input.MouseButtonEventArgs"/> instance containing the event data.</param>
        private void Click_4x3(object sender, MouseButtonEventArgs e)
        {
            this.gameActions.selectedBox(3, 2, true);
        }

        /// <summary>
        /// Manejador para el botón casilla 4x5.
        /// </summary>
        /// <param name="sender">Event.</param>
        /// <param name="e">The <see cref="System.Windows.Input.MouseButtonEventArgs"/> instance containing the event data.</param>
        private void Click_4x5(object sender, MouseButtonEventArgs e)
        {
            this.gameActions.selectedBox(3, 4, true);
        }

        /// <summary>
        /// Manejador para el botón casilla 4x7.
        /// </summary>
        /// <param name="sender">Event.</param>
        /// <param name="e">The <see cref="System.Windows.Input.MouseButtonEventArgs"/> instance containing the event data.</param>
        private void Click_4x7(object sender, MouseButtonEventArgs e)
        {
            this.gameActions.selectedBox(3, 6, true);
        }

        /// <summary>
        /// Manejador para el botón casilla 5x2.
        /// </summary>
        /// <param name="sender">Event.</param>
        /// <param name="e">The <see cref="System.Windows.Input.MouseButtonEventArgs"/> instance containing the event data.</param>
        private void Click_5x2(object sender, MouseButtonEventArgs e)
        {
            this.gameActions.selectedBox(4, 1, true);
        }

        /// <summary>
        /// Manejador para el botón casilla 5x4.
        /// </summary>
        /// <param name="sender">Event.</param>
        /// <param name="e">The <see cref="System.Windows.Input.MouseButtonEventArgs"/> instance containing the event data.</param>
        private void Click_5x4(object sender, MouseButtonEventArgs e)
        {
            this.gameActions.selectedBox(4, 3, true);
        }

        /// <summary>
        /// Manejador para el botón casilla 5x6.
        /// </summary>
        /// <param name="sender">Event.</param>
        /// <param name="e">The <see cref="System.Windows.Input.MouseButtonEventArgs"/> instance containing the event data.</param>
        private void Click_5x6(object sender, MouseButtonEventArgs e)
        {
            this.gameActions.selectedBox(4, 5, true);
        }

        /// <summary>
        /// Manejador para el botón casilla 5x8.
        /// </summary>
        /// <param name="sender">Event.</param>
        /// <param name="e">The <see cref="System.Windows.Input.MouseButtonEventArgs"/> instance containing the event data.</param>
        private void Click_5x8(object sender, MouseButtonEventArgs e)
        {
            this.gameActions.selectedBox(4, 7, true);
        }

        /// <summary>
        /// Manejador para el botón casilla 6x1.
        /// </summary>
        /// <param name="sender">Event.</param>
        /// <param name="e">The <see cref="System.Windows.Input.MouseButtonEventArgs"/> instance containing the event data.</param>
        private void Click_6x1(object sender, MouseButtonEventArgs e)
        {
            this.gameActions.selectedBox(5, 0, true);
        }

        /// <summary>
        /// Manejador para el botón casilla 6x3.
        /// </summary>
        /// <param name="sender">Event.</param>
        /// <param name="e">The <see cref="System.Windows.Input.MouseButtonEventArgs"/> instance containing the event data.</param>
        private void Click_6x3(object sender, MouseButtonEventArgs e)
        {
            this.gameActions.selectedBox(5, 2, true);
        }

        /// <summary>
        /// Manejador para el botón casilla 6x5.
        /// </summary>
        /// <param name="sender">Event.</param>
        /// <param name="e">The <see cref="System.Windows.Input.MouseButtonEventArgs"/> instance containing the event data.</param>
        private void Click_6x5(object sender, MouseButtonEventArgs e)
        {
            this.gameActions.selectedBox(5, 4, true);
        }

        /// <summary>
        /// Manejador para el botón casilla 6x7.
        /// </summary>
        /// <param name="sender">Event.</param>
        /// <param name="e">The <see cref="System.Windows.Input.MouseButtonEventArgs"/> instance containing the event data.</param>
        private void Click_6x7(object sender, MouseButtonEventArgs e)
        {
            this.gameActions.selectedBox(5, 6, true);
        }

        /// <summary>
        /// Manejador para el botón casilla 7x2.
        /// </summary>
        /// <param name="sender">Event.</param>
        /// <param name="e">The <see cref="System.Windows.Input.MouseButtonEventArgs"/> instance containing the event data.</param>
        private void Click_7x2(object sender, MouseButtonEventArgs e)
        {
            this.gameActions.selectedBox(6, 1, true);
        }

        /// <summary>
        /// Manejador para el botón casilla 7x4.
        /// </summary>
        /// <param name="sender">Event.</param>
        /// <param name="e">The <see cref="System.Windows.Input.MouseButtonEventArgs"/> instance containing the event data.</param>
        private void Click_7x4(object sender, MouseButtonEventArgs e)
        {
            this.gameActions.selectedBox(6, 3, true);
        }

        /// <summary>
        /// Manejador para el botón casilla 7x6.
        /// </summary>
        /// <param name="sender">Event.</param>
        /// <param name="e">The <see cref="System.Windows.Input.MouseButtonEventArgs"/> instance containing the event data.</param>
        private void Click_7x6(object sender, MouseButtonEventArgs e)
        {
            this.gameActions.selectedBox(6, 5, true);
        }

        /// <summary>
        /// Manejador para el botón casilla 7x8.
        /// </summary>
        /// <param name="sender">Event.</param>
        /// <param name="e">The <see cref="System.Windows.Input.MouseButtonEventArgs"/> instance containing the event data.</param>
        private void Click_7x8(object sender, MouseButtonEventArgs e)
        {
            this.gameActions.selectedBox(6, 7, true);
        }

        /// <summary>
        /// Manejador para el botón casilla 8X1.
        /// </summary>
        /// <param name="sender">Event.</param>
        /// <param name="e">The <see cref="System.Windows.Input.MouseButtonEventArgs"/> instance containing the event data.</param>
        private void Click_8x1(object sender, MouseButtonEventArgs e)
        {
            this.gameActions.selectedBox(7, 0, true);
        }

        /// <summary>
        /// Manejador para el botón casilla 8x3.
        /// </summary>
        /// <param name="sender">Event.</param>
        /// <param name="e">The <see cref="System.Windows.Input.MouseButtonEventArgs"/> instance containing the event data.</param>
        private void Click_8x3(object sender, MouseButtonEventArgs e)
        {
            this.gameActions.selectedBox(7, 2, true);
        }

        /// <summary>
        /// Manejador para el botón casilla 8x5.
        /// </summary>
        /// <param name="sender">Event.</param>
        /// <param name="e">The <see cref="System.Windows.Input.MouseButtonEventArgs"/> instance containing the event data.</param>
        private void Click_8x5(object sender, MouseButtonEventArgs e)
        {
            this.gameActions.selectedBox(7, 4, true);
        }

        /// <summary>
        /// Manejador para el botón casilla 8x7.
        /// </summary>
        /// <param name="sender">Event.</param>
        /// <param name="e">The <see cref="System.Windows.Input.MouseButtonEventArgs"/> instance containing the event data.</param>
        private void Click_8x7(object sender, MouseButtonEventArgs e)
        {
            this.gameActions.selectedBox(7, 6, true);
        }
    }
}