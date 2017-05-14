using Plugin.MediaManager;
using Plugin.MediaManager.Abstractions.Enums;
using Plugin.MediaManager.Abstractions.Implementations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace AWP.Draw
{
    public class DrawPage : ContentPage
    {
        private readonly StackLayout teamsPanel;
        private readonly StackLayout underStack;
        private readonly StackLayout groupStack;
        private readonly StackLayout group1;
        private readonly StackLayout group2;
        private readonly Label text;
        private readonly ScrollView scrollPanel;
        private readonly Grid grid;
        private readonly Teams.Teams teams;

        public DrawPage()
        {
            teams = new Teams.Teams();
            text = new Label()
            {
                Text = "Zacznij losowanie",
                TextColor = Color.White,
                FontSize=70,
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.CenterAndExpand,
            };

            teamsPanel = new StackLayout()
            {
                Orientation = StackOrientation.Horizontal,
                HeightRequest = 200,
            };
      
            AddTeams();

            scrollPanel = new ScrollView()
            {
                Orientation = ScrollOrientation.Horizontal,
                BackgroundColor = Color.White,
                HeightRequest = 200,
                Content = teamsPanel,
                Margin = new Thickness(0,3,0,0),
            };

            grid = new Grid()
            {              
                Children =
                {
                    scrollPanel,
                    new BoxView(){WidthRequest = 5,HeightRequest = 200,Color = Color.Crimson,HorizontalOptions = LayoutOptions.Center,VerticalOptions = LayoutOptions.Start},              
                }
            };

            var drawButton = new Button()
            {
                Text = "Losuj",
                HeightRequest = 50,
                Margin = new Thickness(350,0,350,0),
                BackgroundColor = Color.Crimson,
                TextColor = Color.White,
            };

            underStack = new StackLayout()
            {
                BackgroundColor = Color.White,
                Children =
                {
                    drawButton,
                }
            };

            drawButton.Clicked += DrawMethod;

            group1 = new StackLayout()
            {
                Margin = new Thickness(5,0,0,0),
                Children =
                {
                    new Label(){Text="Grupa 1:",TextColor = Color.White,FontSize = 50}
                }
            };

            group2 = new StackLayout()
            {
                Margin = new Thickness(40,0,0,0),
                Children =
                {
                    new Label(){Text="Grupa 2:",TextColor = Color.White,FontSize = 50}
                }
            };

            groupStack = new StackLayout()
            {
                IsVisible = false,
                Orientation = StackOrientation.Horizontal,
                VerticalOptions = LayoutOptions.Center,
                Children =
                {
                    group1,group2
                }
            };

            var resultStack = new StackLayout()
            {
                BackgroundColor = Color.FromHex("#2c2c2c"),
                VerticalOptions = LayoutOptions.FillAndExpand,
                Children =
                {
                     text,
                     groupStack,
                     new Image(){Source="sponsorzy.png",VerticalOptions = LayoutOptions.EndAndExpand,HorizontalOptions=LayoutOptions.Center,Margin=new Thickness(0,0,0,5)},
                }
            };

            Content = new StackLayout()
            {
                Children =
                {
                    grid,
                    underStack,
                    resultStack,
                }
            };
        }

        private void DrawMethod(object sender, EventArgs e)
        {
            Sound();
            var rng = new Random();
            scrollPanel.ScrollToAsync(
            rng.Next(Convert.ToInt32((teamsPanel.Children.Count*200)/2), Convert.ToInt32(teamsPanel.Children.Count*200)), 0, true);
            Stop();
        }

        public void Sound()
        {
           CrossMediaManager.Current.Play("http://s1.vocaroo.com/media/download_temp/Vocaroo_s1JtneWwrbRn.mp3");
        }

        public void Stop()
        {
          CrossMediaManager.Current.Pause();
        }
        public void AddTeams()
        {
            var tempList = new List<Grid>();
            var teamsList = teams.getTeams();
            var tempButton = new Grid();

            teamsPanel.Children.Clear();

            if(teamsList.Count==9)
            {
                groupStack.IsVisible = true;

                text.Margin = new Thickness(40, 0, 0, 0);
                text.FontSize = 48;
                text.VerticalOptions = LayoutOptions.CenterAndExpand;
                text.Text = "Po wylosowaniu wszystkich \ndrużyn nautomatycznie zostanie \nrozpisana kolejność meczy";

                groupStack.Children.Add(text);
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
                    teamsPanel.Children.Add(team);
                }
            }
            else 
            {
                Title = "Wyniki losowania:";
                text.IsVisible = false;
                grid.IsVisible = false;
                underStack.IsVisible = false;
                group2.Children.Add(new Button() { Text = teamsList[0], TextColor = Color.White, BackgroundColor = Color.Crimson });
                AddTable();
            }
        }
        private void TeamClicked(object sender, EventArgs e)
        {
            var temp = (Button)sender;

            if (group2.Children.Count < group1.Children.Count)
            {
                group2.Children.Add(new Button() { Text = temp.Text, TextColor = Color.White, BackgroundColor = Color.Crimson });
            }
            else
            {
                group1.Children.Add(new Button() { Text = temp.Text, TextColor = Color.White, BackgroundColor = Color.Crimson });
            }

            teams.RemoveTeam((temp.Text));
            AddTeams();
            scrollPanel.ScrollToAsync(0, 0, false);
        }

        private void AddTable()
        {

            groupStack.HorizontalOptions = LayoutOptions.CenterAndExpand;
            groupStack.VerticalOptions = LayoutOptions.StartAndExpand;
            groupStack.Margin = new Thickness(10,0,0,0);

            var Match = new StackLayout();

            Match.Children.Add(new Label() { Text = "Grupa A:", TextColor = Color.White, FontSize = 24 });
            Match.Children.Add(AddMatch(2, 5, 'a', "8:30"));
            Match.Children.Add(AddMatch(4, 3, 'a', "8:30"));
            Match.Children.Add(AddMatch(5, 1, 'a', "9:30"));
            Match.Children.Add(AddMatch(3, 2, 'a', "9:30"));
            Match.Children.Add(AddMatch(1, 4, 'a', "10:30"));
            Match.Children.Add(AddMatch(5, 3, 'a', "10:30"));
            Match.Children.Add(AddMatch(3, 1, 'a', "11:30"));
            Match.Children.Add(AddMatch(2, 4, 'a', "11:30"));
            Match.Children.Add(AddMatch(1, 2, 'a', "12:30"));
            Match.Children.Add(AddMatch(4, 5, 'a', "12:30"));

            Match.Children.Add(new Label() { Text = "Grupa B:", TextColor = Color.White, FontSize = 24 });
            Match.Children.Add(AddMatch(2, 5, 'b', "14:00"));
            Match.Children.Add(AddMatch(4, 3, 'b', "14:00"));
            Match.Children.Add(AddMatch(5, 1, 'b', "15:00"));
            Match.Children.Add(AddMatch(3, 2, 'b', "15:00"));
            Match.Children.Add(AddMatch(1, 4, 'b', "16:00"));
            Match.Children.Add(AddMatch(5, 3, 'b', "16:00"));
            Match.Children.Add(AddMatch(3, 1, 'b', "17:00"));
            Match.Children.Add(AddMatch(2, 4, 'b', "17:00"));
            Match.Children.Add(AddMatch(1, 2, 'b', "18:00"));
            Match.Children.Add(AddMatch(4, 5, 'b', "18:00"));

            var Matches = new ScrollView()
            {
                Content = Match
            };

            var Table = new StackLayout()
            {
                Margin = new Thickness(40, 0, 0, 0),
                Children =
                {
                   new Label(){Text="Rozpisane mecze:",TextColor = Color.White,FontSize = 50},
                   Matches,
                }
            };

            groupStack.Children.Add(Table);
          
        }

        private StackLayout AddMatch(int team1, int team2,char group,string time)
        {
            var temp1 = new Button();
            var temp2 = new Button();

            if (group=='a')
            {
                temp1 = (Button)group1.Children[team1];
                temp2 = (Button)group1.Children[team2];
            }
            else
            {
                temp1 = (Button)group2.Children[team1];
                temp2 = (Button)group2.Children[team2];
            }

            return new StackLayout()
            {
                Orientation = StackOrientation.Horizontal,
                Children =
                {
                   new Button(){Text=temp1.Text,TextColor=Color.White,BackgroundColor=Color.Crimson,WidthRequest=200},
                   new Button(){Text="vs",TextColor=Color.DimGray,BackgroundColor=Color.White,WidthRequest=40},
                   new Button(){Text=temp2.Text,TextColor=Color.White,BackgroundColor=Color.Crimson,WidthRequest=200},
                   new Button(){Text = $"Dzień: 19 maja, Godzina: {time}",TextColor=Color.White,WidthRequest=250},
                }

            }; 
        }
    }
}