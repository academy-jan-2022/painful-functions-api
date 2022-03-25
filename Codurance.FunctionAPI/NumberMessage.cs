namespace Codurance.FunctionAPI;

using System;

public struct NumberMessage
{
    public int Number;
    public Guid GUID;

    public NumberMessage(int number, Guid GUID) {
        this.Number = number;
        this.GUID = GUID;

    }

} 