
namespace Assets.Scripts.Interface
{
    public interface IShipInput
    {
        float GetAcceleration();  // Ускорение (W / виртуальный джойстик вперед)
        float GetBrake();         // Торможение (S / виртуальный джойстик назад)
        float GetTurn();          // Поворот (A/D или джойстик влево/вправо)

        bool IsPointerOverUI();
    }
}