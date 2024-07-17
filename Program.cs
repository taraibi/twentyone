class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("Welcome to Tuka's Twenty-One game!");

        Deck deck = new Deck();  
        deck.Shuffle();

        List<Card> playerHand = new List<Card>();
        List<Card> dealerHand = new List<Card>();

        playerHand.Add(deck.DrawCard());
        playerHand.Add(deck.DrawCard());
        dealerHand.Add(deck.DrawCard());
        dealerHand.Add(deck.DrawCard());

        bool playerTurn = true;
        bool dealerTurn = true;

        while (playerTurn || dealerTurn)
        {
            if (playerTurn)
            {
                Console.WriteLine("\nYour hand:");
                DisplayHand(playerHand);
                Console.WriteLine("Total value: " + CalculateHandValue(playerHand));
                Console.Write("Do you want to 'hit' or 'stand'? ");
                string playerChoice = Console.ReadLine();

                if (playerChoice.ToLower() == "hit")
                {
                    playerHand.Add(deck.DrawCard());
                    if (CalculateHandValue(playerHand) > 21)
                    {
                        playerTurn = false;
                        dealerTurn = false;
                    }
                }
                else if (playerChoice.ToLower() == "stand")
                {
                    playerTurn = false;
                }
            }

            if (dealerTurn)
            {
                if (CalculateHandValue(dealerHand) < 17)
                {
                    dealerHand.Add(deck.DrawCard());
                }
                else
                {
                    dealerTurn = false;
                }
            }
        }

        Console.WriteLine("\nYour hand:");
        DisplayHand(playerHand);
        Console.WriteLine("Total value: " + CalculateHandValue(playerHand));

        Console.WriteLine("\nDealer's hand:");
        DisplayHand(dealerHand);
        Console.WriteLine("Total value: " + CalculateHandValue(dealerHand));

        DetermineWinner(playerHand, dealerHand);
    }

    static void DisplayHand(List<Card> hand)
    {
        foreach (Card card in hand)
        {
            Console.WriteLine(card);
        }
    }

    static int CalculateHandValue(List<Card> hand)
    {
        int value = 0;
        int aceCount = 0;

        foreach (Card card in hand)
        {
            value += card.Value;
            if (card.Rank == Rank.Ace)
            {
                aceCount++;
            }
        }

        while (value > 21 && aceCount > 0)
        {
            value -= 10;
            aceCount--;
        }

        return value;
    }

    static void DetermineWinner(List<Card> playerHand, List<Card> dealerHand)
    {
        int playerValue = CalculateHandValue(playerHand);
        int dealerValue = CalculateHandValue(dealerHand);

        if (playerValue > 21)
        {
            Console.WriteLine("You busted! Dealer wins.");
        }
        else if (dealerValue > 21)
        {
            Console.WriteLine("Dealer busted! You win.");
        }
        else if (playerValue > dealerValue)
        {
            Console.WriteLine("You win!");
        }
        else if (dealerValue > playerValue)
        {
            Console.WriteLine("Dealer wins.");
        }
        else
        {
            Console.WriteLine("It's a tie!");
        }
    }
}

public class Deck
{
    private List<Card> cards;
    private Random random = new Random();

    public Deck()
    {
        cards = new List<Card>();
        foreach (Suit suit in Enum.GetValues(typeof(Suit)))
        {
            foreach (Rank rank in Enum.GetValues(typeof(Rank)))
            {
                cards.Add(new Card(rank, suit));
            }
        }
    }

    public void Shuffle()
    {
        for (int i = 0; i < cards.Count; i++)
        {
            int j = random.Next(i, cards.Count);
            Card temp = cards[i];
            cards[i] = cards[j];
            cards[j] = temp;
        }
    }

    public Card DrawCard()
    {
        if (cards.Count == 0)
        {
            throw new InvalidOperationException("No cards left in the deck.");
        }

        Card card = cards[0];
        cards.RemoveAt(0);
        return card;
    }
}

public class Card
{
    public Rank Rank { get; }
    public Suit Suit { get; }
    public int Value
    {
        get
        {
            if (Rank >= Rank.Two && Rank <= Rank.Ten)
            {
                return (int)Rank;
            }
            else if (Rank >= Rank.Jack && Rank <= Rank.King)
            {
                return 10;
            }
            else if (Rank == Rank.Ace)
            {
                return 11;
            }
            else
            {
                throw new InvalidOperationException("Unknown card rank.");
            }
        }
    }

    public Card(Rank rank, Suit suit)
    {
        Rank = rank;
        Suit = suit;
    }

    public override string ToString()
    {
        return $"{Rank} of {Suit}";
    }
}

public enum Suit
{
    Hearts,
    Diamonds,
    Clubs,
    Spades
}

public enum Rank
{
    Ace = 1,
    Two = 2,
    Three = 3,
    Four = 4,
    Five = 5,
    Six = 6,
    Seven = 7,
    Eight = 8,
    Nine = 9,
    Ten = 10,
    Jack = 11,
    Queen = 12,
    King = 13
}
