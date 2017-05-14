using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xamarin.Forms;

namespace AWP.Draw
{
    public class DrawPage : ContentPage
    {
        private readonly StackLayout teamsPanel;
        private readonly StackLayout underStack;
        private readonly StackLayout group1;
        private readonly StackLayout group2;
        private readonly ScrollView scrollPanel;
        private readonly Grid grid;
        private readonly Teams.Teams teams;
   
        public DrawPage()
        {
            teams = new Teams.Teams();

            teamsPanel = new StackLayout()
            {
                Orientation = StackOrientation.Horizontal,
                HeightRequest = 200,
            };

            AddTeams();

            scrollPanel = new ScrollView()
            {
                Orientation = ScrollOrientation.Horizontal,
                BackgroundColor = Color.DimGray,
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
                Margin = new Thickness(250,0,250,0),
                BackgroundColor = Color.Crimson,
                TextColor = Color.White,
            };

            underStack = new StackLayout()
            {
                BackgroundColor = Color.DarkGray,
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
                    new Label(){Text="Grupa 1",TextColor = Color.White,FontSize = 50}
                }
            };

            group2 = new StackLayout()
            {
                Margin = new Thickness(40,0,0,0),
                Children =
                {
                    new Label(){Text="Grupa 2",TextColor = Color.White,FontSize = 50}
                }
            };

            var groupStack = new StackLayout()
            {
                Orientation = StackOrientation.Horizontal,
                Children =
                {
                    group1,group2
                }
            };

            Content = new StackLayout()
            {
                Children =
                {
                    grid,
                    underStack,
                    groupStack,
                    new Image(){Source = "sponsorzy.png",VerticalOptions = LayoutOptions.EndAndExpand,HorizontalOptions = LayoutOptions.Center,Margin = new Thickness(3,3,3,3)},
                }
            };
        }

        private void DrawMethod(object sender, EventArgs e)
        {
            var rng = new Random();
            scrollPanel.ScrollToAsync(
                rng.Next(Convert.ToInt32((teamsPanel.Children.Count*200)/2), Convert.ToInt32(teamsPanel.Children.Count*200)), 0, true);
        }

        public void AddTeams()
        {
            var tempList = new List<Grid>();
            var teamsList = teams.getTeams();
            var tempButton = new Grid();

            teamsPanel.Children.Clear();


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
                        };

                        teamButton.Clicked += TeamClicked;

                        tempButton = new Grid()
                        {
                            WidthRequest = 200,
                            HeightRequest = 200,
                            Margin = new Thickness(0, 0, 3, 0),
                            BackgroundColor = Color.White,

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
                underStack.IsVisible = false;
                teamsPanel.IsVisible = false;
                addLastTeam(teamsList[0]);
                grid.Children.RemoveAt(1);
            }
        }
        private void TeamClicked(object sender, EventArgs e)
        {
            var temp = (Button)sender;
            teams.RemoveTeam((temp.Text));
            AddTeams();
            scrollPanel.ScrollToAsync(0, 0, false);

            if (group2.Children.Count < group1.Children.Count)
            {
                group2.Children.Add(new Button(){Text = temp.Text,TextColor = Color.White,BackgroundColor = Color.Crimson});
            }
            else
            {
                group1.Children.Add(new Button() { Text = temp.Text, TextColor = Color.White, BackgroundColor = Color.Crimson });
            }
        }

        private void addLastTeam(string text)
        {
            group2.Children.Add(new Button() { Text = text, TextColor = Color.White, BackgroundColor = Color.Crimson });
        }
    }
}