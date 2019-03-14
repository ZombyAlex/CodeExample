using System;

public class GenerationName
{
	readonly Random rnd;
	public GenerationName()
	{
		rnd = new Random(DateTime.Now.Millisecond);
	}
	bool RandChance(int c)
	{
		return ((rnd.Next(100) + 1) <= c);
	}
	int RandChoice(int i)
	{
		return rnd.Next(i);
	}
	string RandName(int len)
	{
		string aText = "";
		// Very simple markov generator.
		// We repeat letters to make them more likely.
		//const string vowels = "aaaeeeiiiooouuyy'";
		const string vowels = "aaaeeeiiiooouuyy";
		const string frictive = "rsfhvnmz";
		const string plosive = "tpdgkbc";
		const string weird = "qwjx";
		// State transitions..
		// v -> f, p, w, v'
		// v' -> f, p, w
		// f -> p', v
		// p -> v, f'
		// w, p', f' -> v

		int syllables = 0;
		char state;
		int pos = 0;
		bool prime = false;

		// Initial state choice
		if (RandChance(30))
			state = 'v';
		else if (RandChance(40))
			state = 'f';
		else if (RandChance(70))
			state = 'p';
		else
			state = 'w';

		while (pos < len - 1)
		{
			// Apply current state
			switch (state)
			{
				case 'v':
					aText += vowels[RandChoice(vowels.Length)];
					pos++;
					//text[pos++] = vowels[rand_choice(vowels.Length, inRnd)];
					if (!prime)
						syllables++;
					break;
				case 'f':
					aText += frictive[RandChoice(frictive.Length)];
					pos++;
					//text[pos++] = frictive[rand_choice(frictive.Length, inRnd)];
					break;
				case 'p':
					aText += plosive[RandChoice(plosive.Length)];
					pos++;
					// text[pos++] = plosive[rand_choice(plosive.Length, inRnd)];
					break;
				case 'w':
					aText += weird[RandChoice(weird.Length)];
					pos++;
					//text[pos++] = weird[rand_choice(weird.Length, inRnd)];
					break;
			}

			// Chance to stop..
			if (syllables != 0 && pos >= 3)
			{
				if (RandChance(20 + pos * 4))
					break;
			}

			// Transition...
			switch (state)
			{
				case 'v':
					if (!prime && RandChance(10))
					{
						state = 'v';
						prime = true;
						break;
					}
					if (RandChance(40))
						state = 'f';
					else if (RandChance(70))
						state = 'p';
					else
						state = 'w';
					prime = false;
					break;
				case 'f':
					if (!prime && RandChance(50))
					{
						prime = true;
						state = 'p';
						break;
					}
					state = 'v';
					prime = false;
					break;
				case 'p':
					if (!prime && RandChance(10))
					{
						prime = true;
						state = 'f';
						break;
					}
					state = 'v';
					prime = false;
					break;
				case 'w':
					state = 'v';
					prime = false;
					break;
			}
		}
		//text[0] = toupper(text[0]);
		//text[pos++] = '\0';
		aText = char.ToUpper(aText[0]) + aText.Substring(1);
		//aText = aText.Substring(0, 1).ToUpper();
		return aText;
	}
	public string GetName()
	{
		int aLen = rnd.Next(3) + 1;
		string aName = "";
		for (int i = 0; i < aLen; i++)
		{
			aName += RandName(6);
			if (i != aLen - 1)
			{
				if (rnd.Next(3) == 1)
					aName += "-";
				else
					aName += " ";
			}
		}
		return aName;
	}
}