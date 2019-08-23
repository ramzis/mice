public struct Targets
{
    public int remaining;
    public int available;
    public int complete;

    public Targets(int r, int a, int c)
    {
        remaining = r;
        available = a;
        complete = c;
    }
}
