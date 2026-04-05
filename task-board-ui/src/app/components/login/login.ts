import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { AuthService } from '../../shared/services/auth.service';
import { ChangeDetectorRef } from '@angular/core';
import { Router } from '@angular/router';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './login.html',
  styleUrls: ['./login.css']
})
export class LoginComponent {

  email = '';
  password = '';
  name = '';

  showPwd = false;
  loading = false;
  error = '';
  successerror = '';

  isRegisterMode = false; // toggle login/register

  constructor(private auth: AuthService, private cdr: ChangeDetectorRef,private router: Router   ) {}

  // LOGIN
  onLogin() {
  if (!this.email || !this.password) {
    this.error = 'Please fill all fields';
    return;
  }

  this.loading = true;
  this.error = ''; // clear previous error

  this.auth.login(this.email, this.password).subscribe({
    next: (res) => {
      this.loading = false;
       this.auth.setUser(res); 
        localStorage.setItem('token', res.token); 
       this.router.navigate(['/board']);     
    },

   error: (err) => {
  this.loading = false;

  if (err.error?.message) {
    this.error = err.error.message;
  } else {
    this.error = 'Invalid email or password';
  }

  console.log('FINAL ERROR:', this.error);

  // FORCE UI UPDATE
  this.cdr.detectChanges();
}
  });
}
  onInputChange() {
  // only clear if user is actively editing AFTER error is shown
  if (this.error) {
    this.error = '';
  }
}
    // REGISTER
onRegister() {
  if (!this.name || !this.email || !this.password) {
    this.error = 'All fields required';
    return;
  }

  this.loading = true;
  this.error = '';
  this.successerror = '';

  this.auth.register(this.name, this.email, this.password).subscribe({
    next: (res) => {
  this.loading = false;
  this.successerror = 'User created successfully';

  this.cdr.markForCheck(); // better than detectChanges

  setTimeout(() => {   
    this.isRegisterMode = false;
    this.name = '';
    this.email = '';
    this.password = '';
    this.successerror = '';

    this.cdr.markForCheck();
  }, 2000); // ⏳ 2 seconds
},

    error: (err) => {
      this.loading = false;
      this.error = err.error?.message || 'Registration failed ❌';
       this.cdr.markForCheck();
    }
  });
}
}