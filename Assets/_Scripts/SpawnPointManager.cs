public static class SpawnPointManager
{
    public static float timeLastSpawnPoint = 0; // inSeconds
    public static long timeLastSpawnPointMilliseconds = 0; // inMilliseconds
    public static int numberOfCheckpoint = 0;

    public static void SetSpawnPoint(int i, float timeEvent)
    {
        if(Song.instance.stopwatch.ElapsedMilliseconds >= timeEvent)
        {
            timeLastSpawnPoint = (Song.instance.stopwatch.ElapsedMilliseconds + 100) / 1000;
            timeLastSpawnPointMilliseconds = Song.instance.stopwatch.ElapsedMilliseconds + 100;
        }
        else
        {
            timeLastSpawnPoint = (Song.instance.stopwatch.ElapsedMilliseconds + (float)timeLastSpawnPointMilliseconds + 100) / 1000;
            timeLastSpawnPointMilliseconds = Song.instance.stopwatch.ElapsedMilliseconds + timeLastSpawnPointMilliseconds + 100;
        }
        numberOfCheckpoint = i;
    }
}
