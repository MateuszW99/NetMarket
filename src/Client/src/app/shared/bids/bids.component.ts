import { Component, OnInit } from '@angular/core';
import { ItemDetails } from "../items/item-details/item-details.model";
import { Size } from "../size.model";
import { FormControl, FormGroup, Validators } from "@angular/forms";
import { AsksService } from "../asks/asks.service";

@Component({
  selector: 'app-bids',
  templateUrl: './bids.component.html',
  styleUrls: ['./bids.component.css']
})
export class BidsComponent implements OnInit {
  itemDetails: ItemDetails;
  size: Size;
  form: FormGroup;
  userWantsToPlaceBid: boolean;

  constructor(private askService: AsksService) { }

  ngOnInit(): void {
    // TODO: act in case data.X is empty/null
    this.itemDetails = history.state.data.item;
    this.size = history.state.data.size;

    this.form = new FormGroup({
      item: new FormControl(this.itemDetails.item, Validators.nullValidator),
      size: new FormControl('', Validators.nullValidator),
      price: new FormControl(this.itemDetails.highestBid.price, [ Validators.required, Validators.pattern('^[0-9]+(.[0-9]{0,2})?$')]),
    });

    this.userWantsToPlaceBid = this.form.get('price').value >= this.itemDetails.lowestAsk.price;
  }

  getLabel() {
    return this.userWantsToPlaceBid ? 'Place Bid' : 'Buy';
  }

  onSubmitForm(): void {
    if (this.userWantsToPlaceBid) {
      this.onPlaceBid();
    }
    else {
      this.onBuyNow();
    }
  }

  onPlaceBid(): void {

  }

  onBuyNow(): void {

  }

}
