import { Component } from '@angular/core';
import { FormControl, FormGroup, FormsModule, ReactiveFormsModule, Validators } from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatIconModule } from '@angular/material/icon';
import { MatInputModule } from '@angular/material/input';
import { Router } from '@angular/router';
import { AuthService } from '../../services/auth.service';
import { MatSnackBar } from '@angular/material/snack-bar';

@Component({
  selector: 'app-signup',
  imports: [MatButtonModule,MatCardModule,FormsModule, MatFormFieldModule,MatInputModule,ReactiveFormsModule,MatIconModule],
  templateUrl: './signup.component.html',
  styleUrl: './signup.component.css'
})
export class SignupComponent {
  hide = true;
  emailFormControl = new FormControl('', [Validators.required, Validators.email]);
  nameFormControl = new FormControl('', [Validators.required]);
  passwordFormControl = new FormControl('', [Validators.required]);

  userForm = new FormGroup({
    email: this.emailFormControl,
    name: this.nameFormControl,
    password: this.passwordFormControl,
  });

  constructor(private router: Router, private _authService: AuthService, private _snack: MatSnackBar){}

  registrar(){
    if(this.userForm.invalid){
      console.log("invalido");
      return;
    }
    this._authService.register(this.userForm.value).subscribe({
      next: (res) => {
        this.router.navigate(['/login']);
      },
      error: (err) => {
        this._snack.open("Ocurrio un error al procesar", "Cerrar", {duration: 3000})
      }
    });

  }

  cancelar(){
    this.router.navigate(['/login']);
  }
}
