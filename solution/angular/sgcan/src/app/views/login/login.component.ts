import { Component } from '@angular/core';
import { MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatIconModule } from '@angular/material/icon';
import { RouterModule } from '@angular/router';
import { AuthService } from '../../services/auth.service';
import { MatSnackBar } from '@angular/material/snack-bar';

@Component({
  selector: 'app-login',
  imports: [MatButtonModule, MatCardModule, FormsModule, MatFormFieldModule, MatInputModule, ReactiveFormsModule, MatIconModule, RouterModule ],
  templateUrl: './login.component.html',
  styleUrl: './login.component.css'
})
export class LoginComponent {
  hide: boolean = true;

  username: string = '';
  password: string = '';

  user: any = {
    username: undefined,
    password: undefined
  };

  constructor(private _snackBar: MatSnackBar, private _authService: AuthService){}

  handleError(err: any){
    if (err.code==0){
        this._snackBar.open("Error de conexión", "OK", {duration: 3000});
    }else{
        this._snackBar.open(err.error.message, "OK", {duration: 3000});
    }
  }

  login(){
    if (this.username=='' || this.password==''){
        this._snackBar.open("Debe ingresar usuario y contraseña", "OK", {duration: 3000});
        return;
    }
    this.user.username = this.username;
    this.user.password = this.password;

    console.log(this.user)
  }

}
