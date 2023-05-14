namespace SteamProject.Models;

public class GapProcessor {
    
    public int HandlePercent( double percIn )
    {
        double percRound = Math.Round( percIn * 10 ) / 10;

        if( percRound >= 90 )
        {
            return 1;
        }
        else if ( percRound >= 75 )
        {
            return 3;
        }
        else if ( percRound >= 60 )
        {
            return 5;
        }
        else if ( percRound >= 35 )
        {
            return 7;
        }
        else if ( percRound >= 1.1 )
        {
            return 9;
        }
        else {
            return 11;
        }
    }
}