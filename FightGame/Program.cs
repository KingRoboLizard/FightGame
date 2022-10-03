bool option1, option2, option3;
option1 = option2 = option3 = false;
int charNum = 0;
int difficulty = 0;
Console.CursorVisible = false;
Console.OutputEncoding = System.Text.Encoding.UTF8;
int PlayerDamage = 0;
int PlayerHitChance = 0;
int PlayerHealth = 200;
int PlayerMaxHealth = PlayerHealth;
int PlayerPotions = 3;
int score = 0;

int[] EnemyDamage = { 10, 35, 5 };
int[] EnemyHitChance = { 70, 20, 90 };
int[] ArrEnemyHealth = { 100, 150, 200, 300 };
int[] EnemyPotion = { 3, 2, 1, 0 };

ConsoleColor[] colors = { ConsoleColor.Green, ConsoleColor.Yellow, ConsoleColor.Red };

string[] character = { "Char1", "Char2", "Char3", "Char4", "Char5", "Char6" };
string[] enemy = { "Enemy1", "Enemy2", "Enemy3", "Enemy4", "Enemy5" };

Random rand = new Random();
int EnemyRand = rand.Next(enemy.Count() - 1);


options();
Console.Clear();

Console.SetCursorPosition(0, 0);
Console.WriteLine(enemy[EnemyRand] + " has entered the battle!");


int EnemyHealth = ArrEnemyHealth[EnemyRand];
int EnemyPotions = EnemyPotion[EnemyRand];
while (PlayerHealth > 0)
{
    attack();
    if (EnemyHealth < 1)
    {
        PlayerHealth = PlayerMaxHealth;
        EnemyRand = rand.Next(enemy.Count() - 1);
        PlayerPotions = 3;
        EnemyHealth = ArrEnemyHealth[EnemyRand];
        score++;
    }
    else { EnemyAttack(); }
}
Console.Clear();
Console.SetCursorPosition(0, 0);
Console.WriteLine($"Game Over, you killed {score} enemies.");
Console.ReadLine();


void attack()
{
    bool attacking = true;
    int choice = 0;
    Console.SetCursorPosition(0, Console.WindowHeight - 6);
    line(ConsoleColor.Yellow);
    writeHealth(20, Console.WindowHeight - 6, PlayerMaxHealth, PlayerHealth);
    writeHealth(60, Console.WindowHeight - 6, ArrEnemyHealth[EnemyRand], EnemyHealth);
    while (attacking)
    {
        Console.SetCursorPosition(0, Console.WindowHeight - 5);
        Console.WriteLine("  Light Attack");
        Console.WriteLine("  Heavy Attack");
        Console.WriteLine("  Fireball");
        Console.WriteLine($"  Heal {string.Concat(Enumerable.Repeat("\u2666", PlayerPotions))}    ");
        Console.SetCursorPosition(1, choice + Console.WindowHeight - 5);
        Console.Write(">");
        var key = Console.ReadKey(true);
        if (key.Key == ConsoleKey.W && choice > 0) { choice--; }
        else if (key.Key == ConsoleKey.S && choice < 3) { choice++; }
        else if (key.Key == ConsoleKey.Enter || key.Key == ConsoleKey.Spacebar)
        {
            switch (choice)
            {
                case 0:
                    PlayerDamage = 15;
                    PlayerHitChance = 80;
                    attacking = false;
                    break;
                case 1:
                    PlayerDamage = 30;
                    PlayerHitChance = 45;
                    attacking = false;
                    break;
                case 2:
                    PlayerDamage = 10;
                    PlayerHitChance = 100;
                    attacking = false;
                    break;
                case 3:
                    if (PlayerHealth < 150)
                    {
                        if (PlayerPotions > 0)
                        {
                            int PotionHealth = rand.Next(25, 40);
                            PlayerHealth += PotionHealth;
                            PlayerPotions--;
                            Console.SetCursorPosition(0, 2);
                            Console.WriteLine($"You healed {PotionHealth}HP.");
                            Thread.Sleep(1000);
                            Console.SetCursorPosition(0, 2);
                            Console.WriteLine("                             ");
                            attacking = false;
                            break;
                        }
                        else
                        {
                            Console.SetCursorPosition(60, Console.WindowHeight - 5);
                            Console.WriteLine("You're out of health potions!");
                        }
                    }
                    else
                    {
                        Console.SetCursorPosition(60, Console.WindowHeight - 5);
                        Console.WriteLine("You can't heal when you're above 150 health.");
                        break;
                    }
                    break;
            }
        }
    }
    Console.SetCursorPosition(0, 0);
    if (rand.Next(100) < PlayerHitChance && choice != 3)
    {
        EnemyHealth -= PlayerDamage;
        if (EnemyHealth < 1) { Console.WriteLine($"You killed {enemy[EnemyRand]}!               "); }
        else { Console.WriteLine($"You hit {enemy[EnemyRand]} for {PlayerDamage} damage!   "); }
        writeHealth(60, Console.WindowHeight - 6, ArrEnemyHealth[EnemyRand], EnemyHealth);
        Thread.Sleep(1000);
    }
    else if (choice != 3)
    {
        Console.WriteLine("You missed!                   ");
        Thread.Sleep(1000);
    }
}

void EnemyAttack()
{
    Console.SetCursorPosition(0, 0);
    int attack = rand.Next(4);
    if (attack == 3 && EnemyPotions > 0 && EnemyHealth < 70)
    {
        int PotionHealth = rand.Next(20, 30);
        EnemyHealth += PotionHealth;
        EnemyPotions--;
        Console.WriteLine($"Enemy healed {PotionHealth}HP.                      ");
    }
    else if (EnemyPotions == 0 || EnemyHealth > 70)
    {
        attack = rand.Next(3);
    }
    if (attack < 3)
    {
        if (rand.Next(100) < EnemyHitChance[attack])
        {
            PlayerHealth -= EnemyDamage[attack];
            Console.WriteLine($"Enemy hit you for {EnemyDamage[attack]} damage!         ");
        }
        else
        {
            Console.WriteLine("Enemy missed!                            ");
        }
    }
}

static void writeHealth(int x, int y, int MaxHealth, int health)
{
    if (health < 0) { health = 0; }
    Console.ResetColor();
    Console.SetCursorPosition(x, y);
    Console.Write("<");
    for (var i = 0; i < MaxHealth / 10; i++)
    {
        Console.ForegroundColor = ConsoleColor.DarkRed;
        Console.Write("█");
    }
    Console.SetCursorPosition(x + 1, y);
    for (var i = 0; i < health / 10; i++)
    {
        Console.ForegroundColor = ConsoleColor.DarkGreen;
        Console.Write("█");
    }
    Console.ResetColor();
    Console.SetCursorPosition(x + MaxHealth / 10 + 1, y);
    Console.WriteLine($">{health}/{MaxHealth}-");
}

void options()
{
    bool options = true;
    int choice = 0;
    while (options)
    {
        Console.SetCursorPosition(0, Console.WindowHeight - 6);
        line(ConsoleColor.Green);
        writeOption("option1", option1);
        writeOption("option2", option2);
        writeOption("option3", option3);
        writeDiff("difficulty", difficulty);
        characterSel("Player Character:", charNum);
        Console.WriteLine($"  Exit Settings");
        Console.SetCursorPosition(1, choice + Console.WindowHeight - 5);
        Console.Write(">");
        var key = Console.ReadKey(true);
        if (key.Key == ConsoleKey.W && choice > 0) { choice--; }
        else if (key.Key == ConsoleKey.S && choice < 5) { choice++; }
        else if (key.Key == ConsoleKey.Enter || key.Key == ConsoleKey.Spacebar)
        {
            switch (choice)
            {
                case 0:
                    option1 = !option1;
                    break;
                case 1:
                    option2 = !option2;
                    break;
                case 2:
                    option3 = !option3;
                    break;
                case 3:
                    if (difficulty < 2)
                    {
                        difficulty++;
                    }
                    else
                    {
                        difficulty = 0;
                    }
                    break;
                case 4:
                    if (charNum < character.Count() - 1)
                    {
                        charNum++;
                    }
                    else
                    {
                        charNum = 0;
                    }
                    break;
                case 5:
                    options = false;
                    break;
            }
        }
    }
}

void characterSel(string optionName, int charNum)
{
    Console.Write($"  {optionName}");
    Console.ForegroundColor = ConsoleColor.DarkYellow;
    Console.WriteLine($" {character[charNum]} ");
    Console.ResetColor();
}

void writeDiff(string optionName, int difficulty)
{
    Console.Write($"  {optionName}");
    Console.ForegroundColor = colors[difficulty];
    Console.WriteLine($" {difficulty + 1} ");
    Console.ResetColor();
}

void writeOption(string optionName, bool option)
{
    int toInt = option ? 0 : 2;
    Console.Write($"  {optionName}");
    Console.ForegroundColor = colors[toInt];
    Console.WriteLine($" {option} ");
    Console.ResetColor();
}

void line(ConsoleColor color)
{
    Console.ForegroundColor = color;
    for (var i = 0; i < Console.WindowWidth; i++)
    {
        Console.Write("-");
    }
    Console.ResetColor();
}