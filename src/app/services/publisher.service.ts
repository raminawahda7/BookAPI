import { HttpClient } from "@angular/common/http";
import { Injectable } from '@angular/core';
import { Observable } from "rxjs";
import { environment } from "src/environments/environment";
import { Publisher } from './../models/Publisher';

@Injectable({
    providedIn:'root'
})

export class PublisherService{
    private baseUrl:string = environment.baseUrl+'api/';
    
    constructor(private http:HttpClient){}

    public addPublisher(publihser:Publisher){
        return this.http.post(this.baseUrl+'Publishers',publihser);
    }
    public updatePublisher(id:number,publisher:Publisher){
        return this.http.put(this.baseUrl+'Publishers/'+id,publisher)
    }
    public getPublishers():Observable<Publisher[]>{
        return this.http.get<Publisher[]>(this.baseUrl+'Publishes');
    }
    public deletePublisher(id:number){
        return this.http.delete(this.baseUrl+'Publishers/'+id);
    }
    public getPublisherById(id:number):Observable<Publisher>{
        return this.http.get<Publisher>(this.baseUrl+'Publishers/'+id)
    }
    // TODO: Add filter by book and by author too.

}