namespace BackendSanJudas.Interfaces;
public interface IInscripcionPdfServicio { Task<byte[]> GenerarAsync(int idInscripcion,CancellationToken cancelacion); }
