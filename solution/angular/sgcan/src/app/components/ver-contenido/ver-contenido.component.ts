import { Component, inject } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogModule } from '@angular/material/dialog';

@Component({
  selector: 'app-ver-contenido',
  imports: [MatDialogModule],
  templateUrl: './ver-contenido.component.html',
  styleUrl: './ver-contenido.component.css'
})
export class VerContenidoComponent {
  readonly data = inject(MAT_DIALOG_DATA);

  constructor(){}
}
