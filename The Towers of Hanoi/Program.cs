int stackCount = 3;

while (true)
{
    char option = Intro();
    switch(option)
    {
        case 'p':
            Play();
            break;

        case 's':
            Settings();
            break;

        case 'q':
            Environment.Exit(1);
            break;
    } 
}

char Intro()
{
    Console.Clear();
    Console.WriteLine("Welcome To The Towers Of Hanoi \n");
    Console.WriteLine("P : Play");
    Console.WriteLine("S : Settings");
    Console.WriteLine("Q : Quit");
    
    return returnedCharacter(['p', 's', 'q']);
}

char returnedCharacter(char[] AllowedCharacters)
{
    char input = '+';
    char[] allowed = AllowedCharacters;
    while (!allowed.Contains(input))
    {
        input = char.ToLower(Console.ReadKey(true).KeyChar);
    }
    return input;
}

void Settings()
{
    settings:
    Console.Clear();
    Console.WriteLine("Settings \n");
    Console.WriteLine($"C : Stack Count ({stackCount})");
    Console.WriteLine("Q : Return");

    char returned = returnedCharacter(['c', 'q']);

    switch(returned)
    {
        case 'c':
            SettingsStackCount();
            goto settings;
        case 'q':
            return;

    }

    void SettingsStackCount ()
    {
        Console.Clear();
        Console.WriteLine($"Enter New number of stack pieces ({stackCount})");
        string input = Console.ReadLine();
        if (input == "") {return;}
        int newStackCount = int.Parse(input);
        stackCount = newStackCount;
        return;
    }
}

void Play()
{
    int moves = 0;
    bool exit = false;
    Stack<int> Lane1 = new();
    Stack<int> Lane2 = new();
    Stack<int> Lane3 = new();

    Dictionary<int, Stack<int>> convert = new Dictionary<int, Stack<int>>
    {
        {1, Lane1},
        {2, Lane2},
        {3, Lane3}
    };

    int? holder = null;

    for (int i = stackCount; i > 0; i--) // Initializes Placement
    {
        Lane1.Push(i);
    }

    Console.Clear();
    Console.WriteLine($"Towers Of Hanoi | Current Moves ({moves}) | Tower Count ({stackCount}) | Hot Keys (?)");
    Render();
    while (!exit)
    {    
        char keyinput = returnedCharacter(['q', '1', '2', '3', 'r', '?']);
        Console.Clear();
        Console.WriteLine($"Towers Of Hanoi | Current Moves ({moves}) | Tower Count ({stackCount}) | Hot Keys (?)");
        switch (keyinput)
        {
            case '1':
                Move(1);
                break;
            case '2':
                Move(2);
                break;
            
            case '3':
                Move(3);
                break;
            
            case 'q':
                exit = true;
                break;
            
            case 'r':
                Reset();
                break;
            
            case '?':
                Hotkeys();
                break;
        }
        Console.Clear();
        Console.WriteLine($"Towers Of Hanoi | Current Moves ({moves}) | Tower Count ({stackCount}) | Hot Keys (?)");
        Render();
        if (Lane3.Count == stackCount)
        {
            int minimumMoves = (int)Math.Pow(2,stackCount) - 1;
            Console.WriteLine($"Congrats!! You win | Moves Used: {moves} | Minimum possible moves: {minimumMoves}");
            //Console.ReadKey(true);
        }
    }
    exit = false;
    return;

    void Move(int lane)
    {
        if (holder.HasValue) // Has a block in holder
        {

            if (convert[lane].Count > 0 && holder > convert[lane].Peek())
            {
                return;
            }
            convert[lane].Push(holder.Value);
            holder = null;
            moves++;
            
        } else if (convert[lane].Count > 0) // No block in holder
        {
            holder = convert[lane].Pop();
        }
    }

    void Render()
    {
        //Show Raw Values
        Console.WriteLine(string.Join(", ", Lane1));
        Console.WriteLine(string.Join(", ", Lane2));
        Console.WriteLine(string.Join(", ", Lane3));

        int totalwidth = (stackCount*6)-1;
        List<string> rowContents = new();

        for (int i = 0; i<stackCount; i++)
        {
            rowContents.Clear();
            RenderLayer(i);
        }
        return;

        void RenderLayer(int layer)
        {
            List<string> output = new();
            output.Add("|");
            if (Lane1.Count < stackCount-layer && Lane2.Count < stackCount-layer && Lane3.Count < stackCount-layer)
            {
                
                for (int i = 0; i < totalwidth; i++)
                {
                    if (i == stackCount*2 - 1 || i == stackCount*4 - 1)
                    {
                        output.Add("|");
                    } else
                    {
                    output.Add("  ");   
                    }
                }
                output.Add("|");
                Console.WriteLine(string.Join("", output));
                return;
            }
            for (int i = 0; i < 3; i++)
            {
                output.AddRange(PartialLayer(layer,i+1));
                if (i < 2) {output.Add("|");}
            }
            output.Add("|");
            Console.WriteLine(string.Join("",output));
            return;
        }

        List<string> PartialLayer (int layer, int lane)
        {

            //Initializes output list
            int width = (stackCount*2) - 1;
            List<string> output = new List<string>(width);
            for (int i = 0; i < width; i++)
            {
                output.Add("  ");
            }

            //If lane count is less than stack count - layer then there is nothing there
            if (convert[lane].Count < (stackCount - layer))
            {
                return output;
            }

            int diskoffset = convert[lane].Count - (stackCount - layer);
            int diskwidth = convert[lane].ElementAt(diskoffset);
            int diskbuffer = (width - (diskwidth*2 - 1))/2;

            for (int i = 0; i < width; i++)
            {
                if (i < width - diskbuffer && i >= diskbuffer) {output[i] = "##";}
            }
            return output;

        }
    }

    void Hotkeys()
    {
        Console.Clear();
        Console.WriteLine("? : Check Hotkeys");
        Console.WriteLine("R : Reset Game");
        Console.WriteLine("Q : Return To Menu");
        Console.WriteLine("1 : Pick/Place at 1");
        Console.WriteLine("2 : Pick/Place at 2");
        Console.WriteLine("3 : Pick/Place at 3");
        Console.ReadKey(true);
        Console.Clear();
        Console.WriteLine($"Towers Of Hanoi | Current Moves ({moves}) | Tower Count ({stackCount}) | Hot Keys (?)");
        return;
    }

    void Reset()
    {
        moves = 0;

        Lane1.Clear();
        Lane2.Clear();
        Lane3.Clear();

        for (int i = stackCount; i > 0; i--) // Initializes Placement
    {
        Lane1.Push(i);
    }
    }
}


Console.WriteLine("Congrats!! You somehow reached the end of this program \nThis should not be possible \nUpon any press, this game will close");
Console.ReadKey(true);
Environment.Exit(0);