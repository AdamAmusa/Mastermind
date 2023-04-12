using Newtonsoft.Json;

namespace Mastermind;
public class GameState
{
    public int[] playerRows;
    public int playerRow;
    public Color[] savedhiddenColor = new Color[4];
    public Color[] savedguesses = new Color[4];
    public Color[] savedhintPeg;
    

    public GameState()
    {
        int playerRow;
        
        
        
    
    }
}
public partial class MainPage : ContentPage
{
    private string filename = "savedGame.txt";
    private Button CurrentGuess;
    private GameState _gameState;
    static int counter;
    static int resetcounter;
    int r = 11;
    int rowchange = 11;

    public Color[] myColours = { Colors.Red, Colors.Green, Colors.Yellow,
                          Colors.Blue, Colors.DarkOrange,
                          Colors.Purple, Colors.Brown, Colors.Pink, Colors.Blue};
    
    private Random _random;


    public Color[] hiddenColor = new Color[4];
    public Color[] hintPeg = new Color[4];

        
    
    public MainPage()
    {
         

        _random = new Random(); //random class for random colors
        InitializeComponent();
        
    }

    private GameState ReadJsonFile() //function to read file contents to output into the code
    {
        GameState gs = null;
        string jsonText = "";

        try     // reading the local directory (environment.specialfolders)
        {
            string path = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            string fname = Path.Combine(path, filename);
            using (var reader = new StreamReader(fname))
            {
                jsonText = reader.ReadToEnd();
            }
        }
        catch (Exception ex)    // if that fails, then read the embedded resource
        {
            string errorMsg = ex.Message;
        }   // end try catch
        if (jsonText != "")
        {
            gs = new GameState();
            gs = JsonConvert.DeserializeObject<GameState>(jsonText);
            return gs;
        }
        else
            return null;
    }

    private void loadGame_Clicked(object sender, EventArgs e) 
    {
        string fileContent = "";
        _gameState = ReadJsonFile();//function will pass through the variable of the gamestate to allow for file contents to read into the game variables

        if (_gameState != null) //If file exists the game contents of the saved game will load into the game
        {
            hiddenColor[0] = _gameState.savedhiddenColor[0];
            hiddenColor[1] = _gameState.savedhiddenColor[1];
            hiddenColor[2] = _gameState.savedhiddenColor[2];
            hiddenColor[3] = _gameState.savedhiddenColor[3];
            Guess1.BackgroundColor=_gameState.savedguesses[0];
            Guess2.BackgroundColor=_gameState.savedguesses[1];
            Guess3.BackgroundColor=_gameState.savedguesses[2];
            Guess4.BackgroundColor=_gameState.savedguesses[3];
            r = _gameState.playerRow;
            rowchange = r;


            Hiddenpeg1.Color = hiddenColor[0];
            Hiddenpeg2.Color = hiddenColor[1];
            Hiddenpeg3.Color = hiddenColor[2];  
            Hiddenpeg4.Color = hiddenColor[3];

            Gamestart.IsVisible = false;
            Loadgame.IsVisible = false;
            Checkpegs.IsVisible = true;
            Savegame.IsVisible = true;
            MainGame.IsVisible = true;

            GuessRow.SetValue(Grid.RowProperty, r);




        }
        else
        {
            fileContent = "no file found";
        }
        //        LblFileStuff.Text = fileContent;
    }


    public void SaveListOfData(GameState gs)
    {
        string path = Environment.GetFolderPath(
                                       Environment.SpecialFolder.LocalApplicationData); //Saves file into available directory in the users device
        string fname = Path.Combine(path, filename); //assigns name and path of the file
        using (var writer = new StreamWriter(fname, false)) //writes saved contents into the file
        {
            string jsonText = JsonConvert.SerializeObject(gs);
            writer.WriteLine(jsonText);
        }
    }

    private void SaveGame_Clicked(object sender, EventArgs e)
    {

        GameState gs = new GameState(); //class created to to pass saved contents through a function
        gs.savedhiddenColor[0] = hiddenColor[0];
        gs.savedhiddenColor[1] = hiddenColor[1];
        gs.savedhiddenColor[2] = hiddenColor[2];
        gs.savedhiddenColor[3] = hiddenColor[3];
        gs.savedguesses[0] = Guess1.BackgroundColor;
        gs.savedguesses[1] = Guess2.BackgroundColor;
        gs.savedguesses[2] = Guess3.BackgroundColor;
        gs.savedguesses[3] = Guess4.BackgroundColor;
        gs.playerRow = r;
        Savegame.Text = "Game Saved";
        SaveListOfData(gs); //data passes through function
        
    
    }


   

 
    void loadhiddenpegs() //loads four random color pegs for the user to guess
    {
        counter++;
        if(counter == 1)
        {
        hiddenColor[0] = myColours[_random.Next(0, 9)];
        hiddenColor[1] = myColours[_random.Next(0, 9)];
        hiddenColor[2] = myColours[_random.Next(0, 9)];
        hiddenColor[3] = myColours[_random.Next(0, 9)];

        Hiddenpeg1.Color = hiddenColor[0];
        Hiddenpeg2.Color = hiddenColor[1];
        Hiddenpeg3.Color = hiddenColor[2];
        Hiddenpeg4.Color = hiddenColor[3];
        }
    }
 

    private void Button_Clicked(object sender, EventArgs e)
    {
        CurrentGuess = (Button)sender;//white circle is clicked
       

        ColorPicker.IsVisible = true;//color picker becomes visible
        



        }
    private void ColorGuesser_Tapped(object sender, EventArgs e)
    {
        BoxView b = (BoxView)sender; //local variable is assigned to boxview

        CurrentGuess.BackgroundColor = b.Color; //circle that is selected is turned into the color of the circle that was selected from the color picker

        HintPegs(); //The hint pegs are assigned through this function everytime the user gives the white circles color
        
        


        ColorPicker.IsVisible = false; //Color picker becomes invisible until the user clicks a white circle



    }

    private void HintPegs()
    {
    if(CurrentGuess.BackgroundColor.Equals(Guess1.BackgroundColor))
       { 
        if (Guess1.BackgroundColor.Equals(hiddenColor[0])) //If Guess1/CurrentGuess is equal to the first hidden color 
        {
            hintPeg[0] = Colors.Red;// the color of the peg is stored as red into the array



        }
        else if (!Guess1.BackgroundColor.Equals(hiddenColor[0])) //If Guess1/Current Guess is not equal to the first hidden color
        {
            for (int i = 0; i <= 3; i++)
            {
                if (Guess1.BackgroundColor.Equals(hiddenColor[i])) //if Guess1/Current Guess is equal to any other hidden peg
                {
                    hintPeg[0] = Colors.White; //the color of the peg is stored as white
                }
                else
                {
                    hintPeg[0] = Colors.Grey;//If its not that then keep the color of the peg grey
                }
            }
        }
       }

        if (CurrentGuess.BackgroundColor.Equals(Guess2.BackgroundColor))
        {
            if (Guess2.BackgroundColor.Equals(hiddenColor[1]))
            {
                hintPeg[1] = Colors.Red;

            }
            else if (!Guess2.BackgroundColor.Equals(hiddenColor[1]))
            {
                for (int i = 0; i <= 3; i++)
                {
                    if (Guess2.BackgroundColor.Equals(hiddenColor[i]))
                    {
                        hintPeg[1] = Colors.White;
                    }
                    else
                    {
                        hintPeg[1] = Colors.Grey;
                    }
                }
            }
        }

        if (CurrentGuess.BackgroundColor.Equals(Guess3.BackgroundColor))
        {
            if (Guess3.BackgroundColor.Equals(hiddenColor[2]))
            {
                hintPeg[2] = Colors.Red;
            }
            else if (!Guess3.BackgroundColor.Equals(hiddenColor[2]))
            {
                for (int i = 0; i <= 3; i++)
                {
                    if (Guess3.BackgroundColor.Equals(hiddenColor[i]))
                    {
                        hintPeg[2] = Colors.White;
                    }
                    else
                    {
                        hintPeg[2] = Colors.Grey;
                    }
                }
            }
        }


        if (CurrentGuess.BackgroundColor.Equals(Guess4.BackgroundColor))
        {
            if (Guess4.BackgroundColor.Equals(hiddenColor[3]))
            {
                hintPeg[3] = Colors.Red;
            }
            else if (!Guess4.BackgroundColor.Equals(hiddenColor[3]))
            {
                for (int i = 0; i <= 3; i++)
                {
                    if (Guess4.BackgroundColor.Equals(hiddenColor[i]))
                    {
                        hintPeg[3] = Colors.White;
                    }
                    else
                    {
                        hintPeg[3] = Colors.Grey;
                    }
                }
            }
        }

        
    }




    private async void loseGame() //Lose game function
    {
        
           bool answer =  await DisplayAlert("You Lost The Game!", "Would you like to begin a new game", "Yes" , "No");

        if (answer.Equals(0))
        {

        }
        if (answer.Equals(1)) //reset the game if the user selects "Yes"
        {
            resetGame();
        }


    }

 private void moveRow() //Move row function
    {
        resetcounter += 1; //reset counter to prevent the colors from changing to white again until the user moves up a row
  
        r--; //row variable to move row up
       
        Device.StartTimer(new TimeSpan(0, 0, 2), () => //function for the row to move up every 2 seconds after the user selects "check"
        {

            //The Mole will move every two second
            Device.BeginInvokeOnMainThread(() =>
            {
  
                GuessRow.SetValue(Grid.RowProperty, r); //Row value will decrease and the row will go up after the user selects check
                if (resetcounter == 1) //if reset counter is equal to 1
                {
                    Guess1.BackgroundColor = Colors.White; Hintpeg1.Color = Colors.Gray; hintPeg[0] = null; //it sets the values of the new row to "default" values 
                    Guess2.BackgroundColor = Colors.White; Hintpeg2.Color = Colors.Gray; hintPeg[1] = null;
                    Guess3.BackgroundColor = Colors.White; Hintpeg3.Color = Colors.Gray; hintPeg[2] = null;
                    Guess4.BackgroundColor = Colors.White; Hintpeg4.Color = Colors.Gray; hintPeg[3] = null;
                    resetcounter = 0;
                }
                
            });
            return true;
        });
        
    }
    private void resetGame() //reset game function that sets the shown color values to "default" and loads another set of random color pegs and also puts the row value back to 11
    {
        r = 11;
        rowchange = 11;
        GuessRow.SetValue(Grid.RowProperty, r);
        Guess1.BackgroundColor = Colors.White; Hintpeg1.Color = Colors.Gray; hintPeg[0] = null;
        Guess2.BackgroundColor = Colors.White; Hintpeg2.Color = Colors.Gray; hintPeg[1] = null;
        Guess3.BackgroundColor = Colors.White; Hintpeg3.Color = Colors.Gray; hintPeg[2] = null;
        Guess4.BackgroundColor = Colors.White; Hintpeg4.Color = Colors.Gray; hintPeg[3] = null;

        hiddenColor[0] = myColours[_random.Next(0, 9)];
        hiddenColor[1] = myColours[_random.Next(0, 9)];
        hiddenColor[2] = myColours[_random.Next(0, 9)];
        hiddenColor[3] = myColours[_random.Next(0, 9)];

        Hiddenpeg1.Color = hiddenColor[0];
        Hiddenpeg2.Color = hiddenColor[1];
        Hiddenpeg3.Color = hiddenColor[2];
        Hiddenpeg4.Color = hiddenColor[3];

        counter = 2;
    }
    private async void winGame() //win game function
    {   
              bool answer = await DisplayAlert("You Win The Game", "You have guessed all the pegs in the right order, would you like to begin a new game?", "Yes" , "No");
      
        if(answer.Equals(true)){  //if the user selects yes the game will reset its self
                resetGame();
                }

        else
        {

        }
               
    }


    private  void CheckButton_Clicked(object sender, EventArgs e)//Event argument for selecting "Check"
    {
        
        /*The color of the hintpegs are assigned to the main page elements of the hintpefs */
        Hintpeg1.Color = hintPeg[0]; 
        Hintpeg2.Color = hintPeg[1];
        Hintpeg3.Color = hintPeg[2];
        Hintpeg4.Color = hintPeg[3];
        
        if(!Guess1.BackgroundColor.Equals(Colors.White) && !Guess2.BackgroundColor.Equals(Colors.White)
            && !Guess3.BackgroundColor.Equals(Colors.White) && !Guess4.BackgroundColor.Equals(Colors.White)) //If all the guess circles are all not equals to white
        {
            rowchange--; //rowchange variable to calculate the end of the game
            if (Guess1.BackgroundColor.Equals(hiddenColor[0]) && Guess2.BackgroundColor.Equals(hiddenColor[1])
          && Guess3.BackgroundColor.Equals(hiddenColor[2]) && Guess4.BackgroundColor.Equals(hiddenColor[3])) //If all the the Guess are equal to the hidden pegs and they are in right order 
            {
                winGame(); //Function that the user has won the game and the game will reset it self if the user selects yes
                

            }
            
            
           else if(rowchange >= 0) //if the row is less than 0 
            {
                
                moveRow(); //row will move up

            }

           else if(rowchange < 0) //if the row is less than 0
            {
                loseGame(); //the user will lose the game
            }

        }
       






    }

    private void StartGame_Clicked(object sender, EventArgs e) //Start game event argument
    {
        loadhiddenpegs(); //random hidden pegs are loaded
        Gamestart.IsVisible = false; //Game start button becomes invisible
        Loadgame.IsVisible= false; //Load game button becomes invisible
        Checkpegs.IsVisible =true; //Check button becomes visible
        Savegame.IsVisible = true; //Save game buttom becomes visible
        MainGame.IsVisible = true; //Game grid becomes visible
    }
}