import { Component, inject, OnInit, signal } from '@angular/core';
import { ScrapService } from '../../services/scrap.service';
import { FileModel } from '../../models/fileModel';
import { MatIconModule } from '@angular/material/icon';
import { MatExpansionModule } from '@angular/material/expansion';
import {MatProgressBarModule} from '@angular/material/progress-bar';
import { MatButtonModule } from '@angular/material/button';
import { detailsModel } from '../../models/detailsModel';
import { MatCardModule } from '@angular/material/card';
import { MatDialog } from '@angular/material/dialog';
import { VerContenidoComponent } from '../ver-contenido/ver-contenido.component';

@Component({
  selector: 'app-detalles',
  imports: [MatIconModule, MatExpansionModule, MatProgressBarModule, MatButtonModule, MatCardModule],
  templateUrl: './detalles.component.html',
  styleUrl: './detalles.component.css'
})
export class DetallesComponent implements OnInit{

  userId: string;
  fileArray: FileModel[] = [];
  detailsArray: detailsModel[] = [];
  loadingUrls: boolean = false;
  readonly dialog = inject(MatDialog);

  constructor(private _scrapService: ScrapService){
    this.userId = sessionStorage.getItem('id') || '';
  }

  ngOnInit(): void {
    this.fetchFiles();
  }

  fetchFiles(){
    this._scrapService.getFilesByUserId(this.userId).subscribe({
      next: (res) => {
        this.fileArray= res;
      },
      error: (err) => {
        console.log(err);
      }
    })
  }

  loadContent(archivo: string){
    this.loadingUrls = true;
    this._scrapService.getUrlsByFileId(archivo).subscribe({
      next: (res) => {
        this.detailsArray= res;
        this.loadingUrls = false
      },
      error: (err) => {
        console.log(err);
        this.loadingUrls = false
      }
    })
  }
  clearArray(){
    this.detailsArray = [];
  }

  openContent(content: string){
    this.dialog.open(VerContenidoComponent,{
      data: {
        content: content
      }
    })
  }

}
