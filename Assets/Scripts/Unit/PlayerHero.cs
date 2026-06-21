// PlayerHero.cs
using UnityEngine;

public class PlayerHero : Unit
{
    [Header("Компоненты героя")]
    [SerializeField] private SelectableUnit _selectableUnit;
    [SerializeField] private UnitMovement _unitMovement;
    
    protected override void Start()
    {
        base.Start();
        PlayerPool.instance.AddUnitToPool(this.gameObject);
        // Автоматически находим компоненты если не заполнены
        if (_selectableUnit == null)
            _selectableUnit = GetComponent<SelectableUnit>();
        if (_unitMovement == null)
            _unitMovement = GetComponent<UnitMovement>();
    }

    public SelectableUnit GetSelectableUnit() => _selectableUnit;
    public UnitMovement GetUnitMovement() => _unitMovement;
}
