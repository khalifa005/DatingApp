import { AuthService } from './../_service/auth.service';
import { Component, OnInit } from '@angular/core';
import { AlertifyService } from '../_service/alertify.service';

@Component({
  selector: 'app-nav',
  templateUrl: './nav.component.html',
  styleUrls: ['./nav.component.css']
})
export class NavComponent implements OnInit {

  model: any={};
  username: any;
  constructor(public authService: AuthService, private alertify: AlertifyService) { }

  ngOnInit() {

  }



login() {
  this.authService.login(this.model).subscribe(next => {
   this.alertify.success('Logged in successfully');
  }, error => {
    this.alertify.error(error);
  });
}

loggedin() {
 return this.authService.loggedin();
}

logout() {
  localStorage.removeItem('token');
  this.alertify.message('logged out');
}
}
