internal interface IObjectPool
{
    void OnRent();

    void OnReturn();

    void OnClear();
}