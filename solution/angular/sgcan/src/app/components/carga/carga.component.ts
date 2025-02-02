import { Component } from '@angular/core';
import { MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';
import { MatIconModule } from '@angular/material/icon';
import { MatSnackBar } from '@angular/material/snack-bar';
import { MatTooltipModule } from '@angular/material/tooltip';
import { ScrapService } from '../../services/scrap.service';
import { MatProgressBarModule } from '@angular/material/progress-bar';

@Component({
  selector: 'app-carga',
  imports: [MatIconModule, MatButtonModule, MatTooltipModule, MatCardModule, MatProgressBarModule],
  templateUrl: './carga.component.html',
  styleUrl: './carga.component.css'
})
export class CargaComponent {
  filename = '';
  file: any;
  data: any[] = [];
  errors: any[] = [];
  userId: string = '';
  isLoading: boolean = false;

  constructor(private _snack: MatSnackBar, private _scrapService: ScrapService){
    this.userId = sessionStorage.getItem('id') || '';
  }

  onFileChange(event: any) {
    this.errors = [];
    if (!event.target.files) {
      return;
    }
    const file = event.target.files[0];
    this.filename = file.name;
    this.file = file;
  }

  uploadFile() {

    if(this.file == undefined){
      this._snack.open("Debe seleccionar un archivo", "Cerrar", {duration: 3000})
      return;
    }else if(this.file.type != "text/plain"){
      this._snack.open("El archivo debe ser de tipo txt", "Cerrar", {duration: 3000})
      return;
    }
    this.isLoading = true;

    this._scrapService.uploadFile(this.file, this.userId).subscribe({
      next: (res) => {
        this.processFile(res.fileId, res.fileName)
      },
      error: (err) => {
        this.isLoading = false;
        this._snack.open("Error al subir el archivo", "Cerrar", {duration: 3000})
      }
    });
  }
  processFile(id: string, filename: string){
    this._scrapService.processFile({filename:filename,fileId:id}).subscribe({
      next: (res) => {
        this.isLoading = false;
        this._snack.open("Archivo procesado", "Cerrar", {duration: 3000})
      },
      error: (err) => {
        this.isLoading = false;
        this._snack.open("Error al procesar el archivo", "Cerrar", {duration: 3000})
      }
    })
  }
}
