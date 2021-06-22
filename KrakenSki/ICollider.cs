namespace CollisionDetection
{
    public interface ICollider
    {
        // Obter nome do objeto que tem o collider
        string Name();
        // Notificar objeto que houve colisao
        void CollisionWith(ICollider other);
        // Validar colisões (existe/não existe)
        bool CollidesWith(ICollider other);
        // Retorna o collider do objeto (Circle ou OBB)
        ICollider GetCollider();
    }
}