using System.Diagnostics;

public class UnityStopwatch{
    public static Stopwatch timer = new Stopwatch();

    public static string stop(){
        timer.Stop();
        var elapsedTime = timer.Elapsed;
        string output;
        if(elapsedTime.Minutes > 0){
            output = string.Format("{0:00}mins, {1:00}.{2:00}s",
                elapsedTime.Minutes, elapsedTime.Seconds, elapsedTime.Milliseconds / 10);
        }else{
            output = string.Format("{1:00}.{2:00}s",
                elapsedTime.Seconds, elapsedTime.Milliseconds / 10);
        }
        return output;
    }

    public static void start(){
        timer.Reset();
        timer.Start();
    }


}
