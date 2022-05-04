using DLToolkit.Forms.Controls;
using Xamarin.Forms;

namespace AsteroidGame.View
{
    public partial class GamePage : ContentPage
    {
        public GamePage()
        {
            InitializeComponent();
            FlowListView.Init();
        }
    }
}