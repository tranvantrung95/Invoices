import { Component, OnInit } from '@angular/core';
import { AuthService } from './services/auth.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.less']
})
export class AppComponent implements OnInit {
  isCollapsed = false;
  username: string | null = null;
  userId: string | null = null;
  role: string | null = null;
  jtw: any | {};

  constructor(public authService: AuthService) { }

  ngOnInit(): void {
    const userInfo = this.authService.getUserInfo();
    this.username = userInfo.username;
    this.userId = userInfo.userId;
    this.role = userInfo.role;
  }
}
