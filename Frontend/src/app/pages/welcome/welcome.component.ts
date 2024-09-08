import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-welcome',
  templateUrl: './welcome.component.html',
  styleUrls: ['./welcome.component.less']
})
export class WelcomeComponent implements OnInit {

  constructor() { }
  sales = 125000;     // Total sales in dollars
  newOrders = 43;     // Number of new orders
  activeUsers = 1340; // Number of active users

  ngOnInit() {
  }

}
