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

            var underStack = new StackLayout()
            {
                BackgroundColor = Color.White,
                Children =
                {
                    drawButton,
                }
            };

            drawButton.Clicked += DrawMethod;

            Content = new StackLayout()
            {
                Children =
                {
                    grid,
                    underStack,
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
                teamsPanel.Children.Add(new Label(){Text = "Wszystkie drużyny zostały wybrane",TextColor = Color.White,VerticalOptions = LayoutOptions.Center,HorizontalOptions = LayoutOptions.Center,FontSize = 50});
                grid.Children.RemoveAt(1);
            }
        }
        private void TeamClicked(object sender, EventArgs e)
        {
            var temp = (Button)sender;
            teams.RemoveTeam((temp.Text));
            AddTeams();
            scrollPanel.ScrollToAsync(0, 0, false);
        }
    }
}