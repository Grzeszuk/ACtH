using Draw;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.DataTransfer;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.UI.Xaml.Media;
using Xamarin.Forms;

namespace AWP.Draw
{
    public class DrawPage : ContentPage
    {
        #region Variables
        private readonly StackLayout _teamsPanel;
        private readonly StackLayout _underStack;
        private readonly StackLayout _groupStack;
        private readonly StackLayout _group1;
        private readonly StackLayout _group2;
        private readonly Label _text;
        private readonly ScrollView _scrollPanel;
        private readonly Grid _grid;
        private readonly Teams.Teams _teams;
        private readonly List<string> _matches;
        #endregion
        public DrawPage()
        {
            _teams = new Teams.Teams();
            _matches = new List<string>();

            _text = new Label()
            {
                Text = "Zacznij losowanie",
                TextColor = Color.White,
                FontSize=70,
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.CenterAndExpand,
            };

            _teamsPanel = new StackLayout()
            {
                Orientation = StackOrientation.Horizontal,
                HeightRequest = 200,
            };
      
            AddTeams();

            _scrollPanel = new ScrollView()
            {
                Orientation = ScrollOrientation.Horizontal,
                BackgroundColor = Color.White,
                HeightRequest = 200,
                Content = _teamsPanel,
                Margin = new Thickness(0,3,0,0),
            };

            _grid = new Grid()
            {
               
                Children =
                {
                    _scrollPanel,
                    new BoxView(){WidthRequest = 5,HeightRequest = 200,Color = Color.FromRgba(Theme.Accent.R, Theme.Accent.G, Theme.Accent.B, Theme.Accent.A),HorizontalOptions = LayoutOptions.Center,VerticalOptions = LayoutOptions.Start},              
                }
            };

            var drawButton = new Button()
            {
                Text = "Losuj",
                HeightRequest = 50,
                Margin = new Thickness(350,0,350,0),
                BackgroundColor = Color.FromRgba(Theme.Accent.R, Theme.Accent.G, Theme.Accent.B, Theme.Accent.A),
                TextColor = Color.White,
            };

            _underStack = new StackLayout()
            {
                BackgroundColor = Color.White,
                Children =
                {
                    drawButton,
                }
            };

            drawButton.Clicked += DrawMethod;

            _group1 = new StackLayout()
            {
                Margin = new Thickness(5,0,0,0),
                Children =
                {
                    new Label(){Text="Grupa 1:",TextColor = Color.White,FontSize = 50}
                }
            };

            _group2 = new StackLayout()
            {
                Margin = new Thickness(40,0,0,0),
                Children =
                {
                    new Label(){Text="Grupa 2:",TextColor = Color.White,FontSize = 50}
                }
            };

            _groupStack = new StackLayout()
            {
                IsVisible = false,
                Orientation = StackOrientation.Horizontal,
                VerticalOptions = LayoutOptions.Center,
                Children =
                {
                    _group1,_group2
                }
            };

            var resultStack = new StackLayout()
            {
                BackgroundColor = Color.FromHex("#2c2c2c"),
                VerticalOptions = LayoutOptions.FillAndExpand,
                Children =
                {
                     _text,
                     _groupStack,
                     new Image(){Source="sponsorzy.png",VerticalOptions = LayoutOptions.EndAndExpand,HorizontalOptions=LayoutOptions.Center,Margin=new Thickness(0,0,0,5)},
                }
            };

            Content = new StackLayout()
            {
                Children =
                {
                    _grid,
                    _underStack,
                    resultStack,
                }
            };
        }
        private async void DrawMethod(object sender, EventArgs e)
        {
            var rng = new Random();
            await _scrollPanel.ScrollToAsync(
            rng.Next(Convert.ToInt32((_teamsPanel.Children.Count*200)/2), Convert.ToInt32(_teamsPanel.Children.Count*200)), 0, true);
        }
        public void AddTeams()
        {
            var tempList = new List<Grid>();
            var teamsList = _teams.getTeams();
            var tempButton = new Grid();

            _teamsPanel.Children.Clear();

            if(teamsList.Count==9)
            {
                _groupStack.IsVisible = true;

                _text.Margin = new Thickness(40, 0, 0, 0);
                _text.FontSize = 48;
                _text.VerticalOptions = LayoutOptions.CenterAndExpand;
                _text.Text = "Po wylosowaniu wszystkich \ndrużyn nautomatycznie zostanie \nrozpisana kolejność meczy";

                _groupStack.Children.Add(_text);
            }

            if (teamsList.Count > 1)
            {
                foreach (var team in teamsList)
                {
                    for (var y = 0; y < 10; y++)
                    {

                        var teamButton = new Button()
                        {
                            WidthRequest = 200,
                            HeightRequest = 200,
                            Text = team,
                            TextColor = Color.White,
                            FontAttributes = FontAttributes.Bold,
                            BackgroundColor = Color.FromHex("#90000000"),
                        };

                        teamButton.Clicked += TeamClicked;

                        tempButton = new Grid()
                        {
                            
                            WidthRequest = 200,
                            HeightRequest = 200,
                            Margin = new Thickness(0, 0, 3, 0),
                            BackgroundColor = Color.DimGray,

                            Children =
                            {
                                new Image(){Source = "330x192.png",HorizontalOptions = LayoutOptions.Center,VerticalOptions = LayoutOptions.Center},
                                teamButton,
                            }
                        };

                        tempList.Add(tempButton);
                    }             
                }

                foreach (var team in tempList.OrderBy(a => Guid.NewGuid()).ToList())
                {
                    _teamsPanel.Children.Add(team);
                }
            }
            else 
            {
                Title = "Wyniki losowania:";
                _text.IsVisible = false;
                _grid.IsVisible = false;
                _underStack.IsVisible = false;
                _group2.Children.Add(new Button() { Text = teamsList[0], TextColor = Color.White, BackgroundColor = Color.FromRgba(Theme.Accent.R, Theme.Accent.G, Theme.Accent.B, Theme.Accent.A), });
                AddTable();
            }
        }
        private async void TeamClicked(object sender, EventArgs e)
        {
            var temp = (Button)sender;

            if (_group2.Children.Count < _group1.Children.Count)
            {
                _group2.Children.Add(new Button() { Text = temp.Text, TextColor = Color.White, BackgroundColor = Color.FromRgba(Theme.Accent.R, Theme.Accent.G, Theme.Accent.B, Theme.Accent.A), });
            }
            else
            {
                _group1.Children.Add(new Button() { Text = temp.Text, TextColor = Color.White, BackgroundColor = Color.FromRgba(Theme.Accent.R, Theme.Accent.G, Theme.Accent.B, Theme.Accent.A), });
            }

            _teams.RemoveTeam((temp.Text));
            AddTeams();
            await _scrollPanel.ScrollToAsync(0, 0, false);
        }
        private void AddTable()
        {
            _groupStack.HorizontalOptions = LayoutOptions.CenterAndExpand;
            _groupStack.VerticalOptions = LayoutOptions.StartAndExpand;
            _groupStack.Margin = new Thickness(10,0,0,0);

            var match = new StackLayout();

            match.Children.Add(new Label() { Text = "Grupa A:", TextColor = Color.White, FontSize = 24 });
            match.Children.Add(AddMatch(2, 5, 'a', "8:30"));
            match.Children.Add(AddMatch(4, 3, 'a', "8:30"));
            match.Children.Add(AddMatch(5, 1, 'a', "9:30"));
            match.Children.Add(AddMatch(3, 2, 'a', "9:30"));
            match.Children.Add(AddMatch(1, 4, 'a', "10:30"));
            match.Children.Add(AddMatch(5, 3, 'a', "10:30"));
            match.Children.Add(AddMatch(3, 1, 'a', "11:30"));
            match.Children.Add(AddMatch(2, 4, 'a', "11:30"));
            match.Children.Add(AddMatch(1, 2, 'a', "12:30"));
            match.Children.Add(AddMatch(4, 5, 'a', "12:30"));

            match.Children.Add(new Label() { Text = "Grupa B:", TextColor = Color.White, FontSize = 24 });
            match.Children.Add(AddMatch(2, 5, 'b', "14:00"));
            match.Children.Add(AddMatch(4, 3, 'b', "14:00"));
            match.Children.Add(AddMatch(5, 1, 'b', "15:00"));
            match.Children.Add(AddMatch(3, 2, 'b', "15:00"));
            match.Children.Add(AddMatch(1, 4, 'b', "16:00"));
            match.Children.Add(AddMatch(5, 3, 'b', "16:00"));
            match.Children.Add(AddMatch(3, 1, 'b', "17:00"));
            match.Children.Add(AddMatch(2, 4, 'b', "17:00"));
            match.Children.Add(AddMatch(1, 2, 'b', "18:00"));
            match.Children.Add(AddMatch(4, 5, 'b', "18:00"));

            var matches = new ScrollView()
            {
                Content = match
            };

            var table = new StackLayout()
            {
                Margin = new Thickness(40, 0, 0, 0),
                Children =
                {
                   new Label(){Text="Rozpisane mecze:",TextColor = Color.White,FontSize = 50},
                   matches,
                }
            };

            var saveButton = new Button()
            {
                Text = "Zapisz wyniki",
                TextColor = Color.White,
                BackgroundColor = Color.FromRgba(Theme.Accent.R, Theme.Accent.G, Theme.Accent.B, Theme.Accent.A),
            };

            saveButton.Clicked += SaveResult;
            table.Children.Add(saveButton);
            _groupStack.Children.Add(table);
        }
        private async void SaveResult(object sender, EventArgs e)
        {
             var sb = new StringBuilder();
            _matches?.ForEach(x => sb.AppendLine(x));

            var dataPackage = new DataPackage();
            dataPackage.SetText(sb.ToString());
            Clipboard.SetContent(dataPackage);

            try
            {
                var picker = new FileOpenPicker()
                {
                    SuggestedStartLocation = PickerLocationId.DocumentsLibrary,
                    ViewMode = PickerViewMode.List,
                };
                picker.FileTypeFilter.Add(".txt");

                var result = await picker.PickSingleFileAsync();
                await FileIO.AppendTextAsync(result, sb.ToString());
            }
            catch
            {
              await DisplayAlert("Losowanie drużyn - Error", "Bład przy zapisie danych", "OK");
            };
        }
        private StackLayout AddMatch(int team1, int team2,char group,string time)
        {
            var temp1 = group == 'a' ? (Button) _group1.Children[team1] : (Button) _group2.Children[team1];
            var temp2 = group == 'a' ? (Button) _group1.Children[team2] : (Button) _group2.Children[team2];

            _matches.Add($"{temp1.Text} vs {temp2.Text} godzina: {time}");

            return new StackLayout()
            {
                Orientation = StackOrientation.Horizontal,
                Children =
                {
                   new Button(){Text=temp1.Text,TextColor=Color.White,BackgroundColor=Color.FromRgba(Theme.Accent.R, Theme.Accent.G, Theme.Accent.B, Theme.Accent.A),WidthRequest=200},
                   new Button(){Text="vs",TextColor=Color.DimGray,BackgroundColor=Color.White,WidthRequest=40},
                   new Button(){Text=temp2.Text,TextColor=Color.White,BackgroundColor = Color.FromRgba(Theme.Accent.R, Theme.Accent.G, Theme.Accent.B, Theme.Accent.A),WidthRequest=200},
                   new Button(){Text = $"Dzień: 19 maja, Godzina: {time}",TextColor=Color.White,WidthRequest=250},
                }
            }; 
        }
    }
}