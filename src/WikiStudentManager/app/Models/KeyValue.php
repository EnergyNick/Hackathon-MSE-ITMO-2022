<?php

namespace App\Models;

use Illuminate\Database\Eloquent\Model;

class KeyValue extends Model
{
    protected $table = 'key_value';

    /**
     * The attributes that are mass assignable.
     *
     * @var array
     */
    protected $fillable = ['key', 'value',];

    /**
     * The attributes excluded from the model's JSON form.
     *
     * @var array
     */
    protected $hidden = ['id'];

    /**
     * The attributes that cannot be massively assigned.
     *
     * @var array
     */
    protected $guarded = ['id'];

    public $timestamps = false;

    public static function getOfKey(string $key)
    {
        return SELF::where('key', $key)->first()->value;
    }

    public static function setOfKey(string $key, string $value)
    {
        return SELF::updateOrCreate(['key' => $key], ['value' => $value]);
    }
}
