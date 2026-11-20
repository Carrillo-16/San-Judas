# San Judas

Frontend local de administración escolar, React con TypeScript y Tailwind CSS.

```powershell
npm install
npm run dev
```

Abrir http://localhost:5173. Iniciar el backend en http://localhost:5283.
Las peticiones `/Api` se redirigen al backend desde Vite. No utiliza datos simulados.

Compilar: `npm run build`.

Las pantallas están en `src/Paginas`, los componentes compartidos en
`src/Componentes`, la conexión HTTP en `src/Servicios/Api.ts` y los modelos en
`src/Modelos`. Cada pantalla se implementa como una clase React en su archivo TSX.

No hay login en esta etapa; JWT y BCrypt quedan pendientes.
La guía completa está en `../BackendSanJudas/Instrucciones.txt`.
