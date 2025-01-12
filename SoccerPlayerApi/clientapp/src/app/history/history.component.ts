import { Component } from '@angular/core';

@Component({
  selector: 'app-history',
  templateUrl: './history.component.html',
  styleUrls: ['./history.component.css']
})
export class HistoryComponent {
  scopes = [
    { product: "electronics", location: "paris" },
    { product: "home", location: "paris" },
    { product: "clothes", location: "paris" },
    { product: "electronics", location: "lyon" },
    { product: "home", location: "lyon" },
    { product: "clothes", location: "lyon" },
    { product: "electronics", location: "marseille" },
    { product: "sports", location: "marseille" },
    { product: "clothes", location: "marseille" },
    { product: "electronics", location: "toulon" },
    { product: "sports", location: "toulon" },
    { product: "clothes", location: "toulon" },
  ];
}
