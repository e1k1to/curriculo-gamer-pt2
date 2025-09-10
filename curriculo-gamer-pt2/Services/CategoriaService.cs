using curriculo_gamer_pt2.Exceptions;
using curriculo_gamer_pt2.Models.Context;
using curriculo_gamer_pt2.Models.Entities;
using curriculo_gamer_pt2.Models.Interfaces;

namespace curriculo_gamer_pt2.Services
{
    public class CategoriaService : ICategoriaService
    {
        private readonly LogContext _context;
        public CategoriaService(LogContext context)
        {
            _context = context;
        }
        
        public Categoria? BuscarPorId(int id)
        {
            return _context.Categorias.Find(id);
        }

        public bool Deletar(int id)
        {
            Categoria? categoriaBuscada = BuscarPorId(id);
            if (categoriaBuscada == null)
                return false;
            _context.Categorias.Remove(categoriaBuscada);
            _context.SaveChanges();
            return true;
        }

        public Categoria Incluir(Categoria categoria)
        {
            if (categoria == null)
                throw new ObjectNotFoundException("Categoria nula");

            _context.Categorias.Add(categoria);
            _context.SaveChanges();

            return categoria;
        }

        public List<Categoria> ListarTodos()
        {
            return _context.Categorias.ToList();
        }

        public List<Categoria> BuscarPorIds(List<int> ids)
        {
            return _context.Categorias.Where(c => ids.Contains(c.Id)).ToList();
        }
    }
}
