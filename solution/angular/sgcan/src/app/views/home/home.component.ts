import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { MatToolbarModule } from '@angular/material/toolbar';
import { MatButtonModule } from '@angular/material/button';
import { MatTabsModule } from '@angular/material/tabs';
import { CargaComponent } from "../../components/carga/carga.component";
import { DetallesComponent } from "../../components/detalles/detalles.component";

@Component({
  selector: 'app-home',
  imports: [MatToolbarModule, MatTabsModule, MatButtonModule, CargaComponent, DetallesComponent],
  templateUrl: './home.component.html',
  styleUrl: './home.component.css'
})
export class HomeComponent implements OnInit{

  name: string;
  uId: string;
  selectedIndex: number = 0;

  ngOnInit(): void {

    if (this.name == '' || this.uId == ''){
      this.router.navigate(['/login']);
    }
    console.log(this.name, this.uId);

  }
  constructor(private router: Router) {
    this.name = sessionStorage.getItem('name') || ''
    this.uId = sessionStorage.getItem('id') || '';
  }

  logOut() {
    sessionStorage.clear();
    this.router.navigate(['/login']);
  }

}
