using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IIdentifiable
{
    int Id { get; }
    public void SetId(int id);
}
